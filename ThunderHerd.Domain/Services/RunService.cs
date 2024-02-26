using Microsoft.Extensions.Options;
using ThunderHerd.Core;
using ThunderHerd.Core.Extensions;
using ThunderHerd.Core.Models.Dtos;
using ThunderHerd.Core.Models.Settings;
using ThunderHerd.Core.Options;
using ThunderHerd.Domain.Interfaces;

namespace ThunderHerd.Domain.Services
{
    public class RunService : IRunService
    {
        private readonly IOptions<TestServiceOptions> _options;
        private readonly ITestResultService _testResultService;
        private readonly ITestService _testService;
        private readonly IHerdClient _client;

        public RunService(IOptions<TestServiceOptions> options,
            ITestResultService testResultService,
            ITestService testService,
            IHerdClient client)
        {
            _options = options;
            _testService = testService;
            _testResultService = testResultService;
            _client = client;
        }

        public async Task RunTestAsync(Guid testId, CancellationToken cancellationToken)
        {
            var entity = await _testService.GetTestAsync(testId, cancellationToken);
            await RunTestAsync(entity, cancellationToken);
        }

        public async Task RunTestAsync(Test? test, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(test, nameof(test));

            // Clamp values to min and max
            var runDurationInSeconds = test.RunDurationInSeconds > 0
                ? test.RunDurationInSeconds
                : 0;
            runDurationInSeconds = runDurationInSeconds > _options.Value.MaxRunDurationInSeconds
                ? _options.Value.MaxRunDurationInSeconds
                : test.RunDurationInSeconds;

            var warmupDurationInSeconds = test.WarmupDurationInSeconds > 0
                ? test.WarmupDurationInSeconds
                : 0;
            warmupDurationInSeconds = warmupDurationInSeconds > _options.Value.MaxWarmupDurationInSeconds
                ? _options.Value.MaxWarmupDurationInSeconds
                : test.WarmupDurationInSeconds;

            // Make sure that the total duration is at least as large as the
            // warmup phase
            runDurationInSeconds = warmupDurationInSeconds > runDurationInSeconds
                ? warmupDurationInSeconds
                : runDurationInSeconds;

            // Calculate the total run time
            var runDuration = TimeSpan.FromSeconds(runDurationInSeconds);
            var warmupDuration = TimeSpan.FromSeconds(warmupDurationInSeconds);

            // Clamp callsPerSecond to min and max values
            var callsPerSecond = test.CallsPerSecond > 1
                ? test.CallsPerSecond
                : 1;
            callsPerSecond = callsPerSecond > _options.Value.MaxCallsPerSecond
                ? _options.Value.MaxCallsPerSecond
                : callsPerSecond;

            // Calculate how many calls to make each second during the warmup phase
            var numCallsToAddPerSecond = Convert.ToDouble(callsPerSecond);

            // If we have no warmup phase we ramp up to max calls right away
            if (warmupDurationInSeconds > 0)
            {
                numCallsToAddPerSecond = callsPerSecond / warmupDurationInSeconds > 0
                ? callsPerSecond / warmupDurationInSeconds // Calculate calls per second with warmup
                : callsPerSecond; // Calculate calls per second without warmup
            }

            // Create new request options for httpClient
            var requestSettings = new HerdClientRequestSettings
            {
                //AppId = test.AppId,
                //AppSecret = test.AppSecret,
                //ApiKey = test.ApiKey,
            };

            // Calculate start and end times
            var runStart = DateTime.Now;
            var runEnd = runStart + runDuration;

            // Create TestResult stub and save to DB
            var testResult = new TestResult
            {
                TestId = test.Id,
                RunStarted = runStart,
                WarmupDuration = warmupDuration,
                Status = Core.Enums.TestStatus.Running
            };
            testResult = await _testResultService.CreateTestResultAsync(testResult, cancellationToken);
            ArgumentNullException.ThrowIfNull(testResult);

            // Begin the test run
            // Add at least one call step
            var step = 0;
            var callStep = 1;
            var tasks = new List<Task<HttpResponseMessage>>();
            do
            {
                // If this is the first step (step 0) then we always make requests 
                // Or if a second has passed then we also make more requests
                if (step == 0 || (runStart + TimeSpan.FromSeconds(step) < DateTime.Now))
                {
                    // Make calls and add to task list
                    tasks.AddRange(
                        MakeRequest(callStep,
                        numCallsToAddPerSecond,
                        test.TestItems,
                        requestSettings,
                        cancellationToken));

                    // If we have reached the max number of calls per second, we stop at that callStep
                    // and don't increase it any further since we don't want to increase the calls each step anymore
                    if (callStep * numCallsToAddPerSecond <= callsPerSecond)
                    {
                        callStep++;
                    }

                    step++;
                }
            }
            while (runEnd > DateTime.Now && !cancellationToken.IsCancellationRequested);

            // Set the runtime if the call was cancelled instead of allowed to run to it's conclusion
            runEnd = DateTime.Now;

            // If cancellation has been requested; stop everything, save what we can and exit quickly
            if (cancellationToken.IsCancellationRequested)
            {
                testResult.Status = Core.Enums.TestStatus.Cancelled;
                testResult.RunDuration = runEnd - runStart;
                testResult = await _testResultService.UpdateTestResultAsync(testResult, CancellationToken.None);
                return;
            }

            // Wait until all requests has been returned
            var responseList = await Task.WhenAll(tasks);

            // Set the runtime after all requests has finished
            runEnd = DateTime.Now;

            // Update test result and save to DB
            testResult.RunCompleted = runEnd;
            testResult.RunDuration = runEnd - runStart;
            testResult.WarmupDuration = warmupDuration;
            testResult.Status = Core.Enums.TestStatus.Completed;
            testResult = await _testResultService.UpdateTestResultAsync(testResult, cancellationToken);

            // Map each individual test result
            var timeSpan = TimeSpan.FromSeconds(_options.Value.TimeSlotSpanInSeconds);
            var testResultItems = responseList
                    .OrderBy(p => long.Parse(p.RequestMessage.GetHeaderValue(Globals.HeaderNames.StartTimeInTicks)))
                    .Select(p => TestResultItem.Map(testResult.Id, p));

            // Save results to DB
            await _testResultService.CreateTestResultItems(testResultItems, cancellationToken);
        }

        private IEnumerable<Task<HttpResponseMessage>> MakeRequest(
            int callStep,
            double numCallsToAddPerSecond,
            IEnumerable<TestItem> testList,
            HerdClientRequestSettings? settings = default,
            CancellationToken cancellationToken = default)
        {
            // Round up so that number of calls are never below 1
            var numCallsToAdd = Convert.ToInt32(Math.Ceiling(callStep * numCallsToAddPerSecond));

            // TODO: Gotta go fast! Might want to look into makes this a multi thread by using parallel processing, but there might be trade-offs
            for (var i = 0; i < numCallsToAdd; i++)
            {
                foreach (var testLink in testList)
                {
                    if (string.IsNullOrEmpty(testLink.Url))
                        continue;

                    yield return _client.SendAsync(testLink.Url, testLink.Method, settings, cancellationToken);
                }
            }
        }
    }
}

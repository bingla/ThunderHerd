using Microsoft.Extensions.Options;
using ThunderHerd.Core;
using ThunderHerd.Core.Extensions;
using ThunderHerd.Core.Models.Dtos;
using ThunderHerd.Core.Models.Settings;
using ThunderHerd.Core.Options;
using ThunderHerd.DataAccess.Interfaces;
using ThunderHerd.Domain.Interfaces;

namespace ThunderHerd.Domain.Services
{
    public class RunService : IRunService
    {
        private readonly IOptions<RunServiceOptions> _options;
        private readonly IRunRepository _runRepository;
        private readonly IHerdClient _client;

        public RunService(
            IOptions<RunServiceOptions> options,
            IRunRepository runRepository,
            IHerdClient client)
        {
            _options = options;
            _runRepository = runRepository;
            _client = client;
        }

        /// <summary>
        /// Load a scheduled run from DB and start it
        /// </summary>
        /// <param name="runId">Id of run</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<RunResult> RunAsync(Guid runId, CancellationToken cancellationToken)
        {
            var entity = await _runRepository.FindAsync(runId, cancellationToken);
            return await RunAsync(Run.Map(entity), cancellationToken);
        }

        /// <summary>
        /// Start a test run
        /// </summary>
        /// <param name="run">Run object</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>RunResult</returns>
        public async Task<RunResult> RunAsync(Run run, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(run, nameof(run));

            // Clamp values to min and max
            var runDurationInMinutes = run.RunDurationInMinutes > 0
                ? run.RunDurationInMinutes
                : 0;
            runDurationInMinutes = runDurationInMinutes > _options.Value.MaxRunDurationInMinutes
                ? _options.Value.MaxRunDurationInMinutes
                : run.RunDurationInMinutes;

            var warmupDurationMinutes = run.WarmupDurationInMinutes > 0
                ? run.WarmupDurationInMinutes
                : 0;
            warmupDurationMinutes = warmupDurationMinutes > _options.Value.MaxWarmupDurationInMinutes
                ? _options.Value.MaxWarmupDurationInMinutes
                : run.WarmupDurationInMinutes;

            // Make sure that the total duration is at least as large as the
            // warmup phase
            runDurationInMinutes = warmupDurationMinutes > runDurationInMinutes
                ? warmupDurationMinutes
                : runDurationInMinutes;

            // Calculate the total run time
            var runDuration = TimeSpan.FromMinutes(runDurationInMinutes);
            var warmupDuration = TimeSpan.FromMinutes(warmupDurationMinutes);
            var warmupDurationInSeconds = warmupDuration.TotalSeconds;

            // Clamp callsPerSecond to min and max values
            var callsPerSecond = run.CallsPerSecond > 1
                ? run.CallsPerSecond
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
                ? callsPerSecond / warmupDurationInSeconds // Calculate calls with warmup
                : callsPerSecond; // Calculate with no warmup
            }

            // Create new request options for httpClient
            var requestSettings = new HerdClientRequestSettings
            {
                AppId = run.AppId,
                AppSecret = run.AppSecret,
                ApiKey = run.ApiKey,
            };

            // Begin the test run
            // Add at least one call step
            var runStart = DateTime.Now;
            var runEnd = runStart + runDuration;
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
                    tasks.AddRange(MakeRequest(callStep, numCallsToAddPerSecond,
                        run.TestCollection, requestSettings, cancellationToken));

                    // If we have reached the max number of calls her second, we stop at that callStep
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

            // Wait until all requests has been returned
            var responseList = await Task.WhenAll(tasks);

            // Group and calculate results
            var timeSpan = TimeSpan.FromSeconds(_options.Value.TimeSlotSpanInSeconds);
            var resultList = responseList
                .OrderBy(p => long.Parse(p.RequestMessage.GetHeaderValue(Globals.HeaderNames.StartTimeInTicks)) / timeSpan.Ticks)
                .GroupBy(p => long.Parse(p.RequestMessage.GetHeaderValue(Globals.HeaderNames.StartTimeInTicks)) / timeSpan.Ticks, p => p)
                .Select(p => RunResult.TestResultSlotItem.Map(p, timeSpan))
                .OrderBy(p => p.Tick)
                .ToHashSet();

            return new RunResult
            {
                RunStarted = runStart,
                RunCompleted = runEnd,
                RunDuration = runEnd - runStart,
                WarmupDuration = warmupDuration,
                TimeSlotCollection = resultList,
            };
        }

        private IEnumerable<Task<HttpResponseMessage>> MakeRequest(int callStep,
            double numCallsToAddPerSecond,
            IEnumerable<Run.TestItem> testList,
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

using ThunderHerd.Core.Models.Dtos;
using ThunderHerd.Domain.Interfaces;

namespace ThunderHerd.Domain.Services
{
    public class RunService : IRunService
    {
        private readonly IHerdClient _client;

        public RunService(IHerdClient client)
        {
            _client = client;
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

            // Clamp values
            var runDurationInMinutes = run.RunDurationInMinutes <= 0 ? 0 : run.RunDurationInMinutes;
            var warmupDurationMinutes = run.WarmupDurationInMinutes < 0 ? 0 : run.WarmupDurationInMinutes;

            // Make sure that the total duration is at least as large as the
            // warmup phase
            runDurationInMinutes = warmupDurationMinutes > runDurationInMinutes
                ? warmupDurationMinutes
                : runDurationInMinutes;

            // Calculate the total run time
            var runDuration = TimeSpan.FromMinutes(runDurationInMinutes);
            var warmupDuration = TimeSpan.FromMinutes(warmupDurationMinutes);
            var warmupDurationInSeconds = warmupDuration.TotalSeconds;

            // Calculate how many calls to make each second during the warmup phase
            var callsPerSecond = run.CallsPerSecond <= 0 ? 1 : run.CallsPerSecond;
            var numCallsToAddPerSecond = Convert.ToDouble(callsPerSecond);

            // If we have no warmup phase we ramp up to max calls right away
            if (warmupDurationInSeconds > 0)
            {
                numCallsToAddPerSecond = callsPerSecond / warmupDurationInSeconds > 0
                ? callsPerSecond / warmupDurationInSeconds // Calculate calls with warmup
                : callsPerSecond; // Calculate with no warmup
            }

            // Begin the test run
            // and to at least one call step
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
                    tasks.AddRange(MakeRequest(callStep,numCallsToAddPerSecond,
                        run.TestCollection, cancellationToken));

                    // If we have reached the max number of calls her second, we stop at that callStep
                    // and don't increase it any further
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
            var resultList = responseList
                .GroupBy(p => p.RequestMessage?.RequestUri, p => p)
                .Select(RunResult.TestResultItem.Map)
                .ToHashSet();

            return new RunResult
            {
                RunStarted = runStart,
                RunCompleted = runEnd,
                RunDuration = runEnd - runStart,
                WarmupDuration = warmupDuration,
                Results = resultList,
            };
        }
        
        private IEnumerable<Task<HttpResponseMessage>> MakeRequest(int callStep, double numCallsToAddPerSecond,
            IEnumerable<Run.TestItem> testList, CancellationToken cancellationToken = default)
        {
            // Round up so that number of calls are never 0
            var numCallsToAdd = Convert.ToInt32(Math.Ceiling(callStep * numCallsToAddPerSecond));

            // TODO: Makes this a multi thread by using parallel processing, gottago fast!
            for (var i = 0; i < numCallsToAdd; i++)
            {
                foreach (var url in testList)
                {
                    yield return _client.SendAsync(url.Url, url.Method, cancellationToken);
                }
            }
        }
    }
}

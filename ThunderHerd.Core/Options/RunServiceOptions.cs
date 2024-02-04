namespace ThunderHerd.Core.Options
{
    public class RunServiceOptions
    {
        public int MaxParallelThreads { get; set; } = 4;
        public int MaxCallsPerSecond { get; set; } = 100;
        public int MaxRunDurationInMinutes { get; set; } = 60;
        public int MaxWarmupDurationInMinutes { get; set; } = 60;
    }
}

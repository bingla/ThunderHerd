namespace ThunderHerd.Core.Options
{
    public class TestServiceOptions
    {
        public int MaxParallelThreads { get; set; } = 4;
        public int MaxCallsPerSecond { get; set; } = 20;
        public int MaxRunDurationInSeconds { get; set; } = 3600;
        public int MaxWarmupDurationInSeconds { get; set; } = 3600;
        public int TimeSlotSpanInSeconds { get; set; } = 1;
    }
}

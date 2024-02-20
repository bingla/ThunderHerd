namespace ThunderHerd.Core.Entities
{
    public class Test
    {
        public Guid Id { get; set; }
        public int CallsPerSecond { get; set; } = 1;
        public int RunDurationInSeconds { get; set; } = 1;
        public int WarmupDurationInSeconds { get; set; }

        public IEnumerable<TestItem> TestItems { get; init; } = new HashSet<TestItem>();

        public static Test Map(Models.Dtos.Test entity)
        {
            return new Test
            {
                Id = entity.Id,
                CallsPerSecond = entity.CallsPerSecond,
                RunDurationInSeconds = entity.RunDurationInSeconds,
                WarmupDurationInSeconds = entity.WarmupDurationInSeconds,
                TestItems = entity.TestItems.Select(TestItem.Map).ToHashSet(),
            };
        }

        public record RunId(Guid Id);
    }
}

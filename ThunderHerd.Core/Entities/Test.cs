namespace ThunderHerd.Core.Entities
{
    public class Test
    {
        public Guid Id { get; set; }
        public int CallsPerSecond { get; set; } = 1;
        public int RunDurationInMinutes { get; set; } = 1;
        public int WarmupDurationInMinutes { get; set; }

        public IEnumerable<TestItem> TestItems { get; init; } = new HashSet<TestItem>();

        public static Test Map(Models.Dtos.Test entity)
        {
            return new Test
            {
                Id = entity.Id,
                CallsPerSecond = entity.CallsPerSecond,
                RunDurationInMinutes = entity.RunDurationInMinutes,
                WarmupDurationInMinutes = entity.WarmupDurationInMinutes,
                TestItems = entity.TestItems.Select(TestItem.Map).ToHashSet(),
            };
        }

        public record RunId(Guid Id);
    }
}

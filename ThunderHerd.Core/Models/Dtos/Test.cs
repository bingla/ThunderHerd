using ThunderHerd.Core.Models.Requests;

namespace ThunderHerd.Core.Models.Dtos
{
    public partial class Test
    {
        public Guid Id { get; set; }
        public int CallsPerSecond { get; set; } = 1;
        public int RunDurationInMinutes { get; set; } = 1;
        public int WarmupDurationInMinutes { get; set; }

        public IEnumerable<TestItem> TestItems { get; init; } = new HashSet<TestItem>();

        public static Test Map(TestCreateRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            return new Test
            {
                CallsPerSecond = request.CallsPerSecond,
                RunDurationInMinutes = request.RunDurationInMinutes,
                WarmupDurationInMinutes = request.WarmupDurationInMinutes,
                TestItems = request.TestCollection.Select(TestItem.Map).ToHashSet(),
            };
        }

        public static Test? Map(Entities.Test? entity)
        {
            if (entity == default)
                return default;

            return new Test
            {
                Id = entity.Id,
                CallsPerSecond = entity.CallsPerSecond,
                RunDurationInMinutes = entity.RunDurationInMinutes,
                WarmupDurationInMinutes = entity.WarmupDurationInMinutes,
                TestItems = entity.TestItems.Select(TestItem.Map),
            };
        }
    }
}

using ThunderHerd.Core.Enums;
using ThunderHerd.Core.Models.Dtos;

namespace ThunderHerd.Core.Models.Responses
{
    public class TestResultResponse
    {
        public Guid Id { get; set; }
        public DateTime RunStarted { get; set; }
        public DateTime RunCompleted { get; set; }
        public TimeSpan RunDuration { get; set; }
        public TimeSpan WarmupDuration { get; set; }
        public TestStatus Status { get; set; } = TestStatus.NA;

        public long NumTotalCalls { get; set; }
        public long NumSuccessCalls { get; set; }
        public long NumErrorCalls { get; set; }


        public static TestResultResponse? Map(TestResult? entity)
        {
            if (entity == default)
                return default;

            return new TestResultResponse
            {
                Id = entity.Id,
                RunStarted = entity.RunStarted,
                RunCompleted = entity.RunCompleted,
                RunDuration = entity.RunDuration,
                WarmupDuration = entity.WarmupDuration,
                Status = entity.Status
            };
        }
    }
}

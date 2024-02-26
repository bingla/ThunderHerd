using System.ComponentModel.DataAnnotations;
using ThunderHerd.Core.Enums;
using ThunderHerd.Core.Models.Dtos;

namespace ThunderHerd.Core.Models.Responses
{
    public class TestCreateResponse
    {
        public Guid Id { get; set; }
        public int CallsPerSecond { get; set; } = 1;
        public int RunDurationInMinutes { get; set; }
        public int WarmupDurationInMinutes { get; set; }

        public IEnumerable<TestItem> TestCollection { get; init; } = new HashSet<TestItem>();

        public static TestCreateResponse? Map(Test? entity)
        {
            if (entity == default)
                return default;

            return new TestCreateResponse
            {
                Id = entity.Id,
                CallsPerSecond = entity.CallsPerSecond,
                RunDurationInMinutes = entity.RunDurationInSeconds,
                WarmupDurationInMinutes = entity.WarmupDurationInSeconds,
                TestCollection = entity.TestItems.Select(TestItem.Map).ToHashSet(),
            };
        }

        public class TestItem
        {
            [Required]
            public HttpMethods Method { get; set; } = HttpMethods.GET;
            [Required]
            public string? Url { get; set; }

            public static TestItem Map(Dtos.TestItem entity)
            {
                return new TestItem
                {
                    Method = entity.Method,
                    Url = entity.Url,
                };
            }
        }
    }
}

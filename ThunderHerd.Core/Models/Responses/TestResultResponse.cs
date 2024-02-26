using System.Net;
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

        public class TestResultSlotItem
        {
            public long Tick { get; set; }
            public TimeSpan TimeSpan { get; set; }
            public IEnumerable<TestResultGroupItem> ResultGroupCollection { get; init; } = new HashSet<TestResultGroupItem>();

            public static TestResultSlotItem Map(TestResult.TestResultSlotItem item)
            {
                return new TestResultSlotItem
                {
                    Tick = item.Tick,
                    TimeSpan = item.TimeSpan,
                    ResultGroupCollection = item.ResultGroupCollection
                        .Select(TestResultGroupItem.Map)
                        .ToHashSet(),
                };
            }

            public class TestResultGroupItem
            {
                public HttpMethods Method { get; set; }
                public string? Host { get; set; }
                public string? Url { get; set; }
                public string? Query { get; set; }
                public string? AbsoluteUrl { get; set; }
                public long TotalCount => SuccessCount + ErrorCount;
                public int SuccessCount { get; set; }
                public int ErrorCount { get; set; }

                public decimal MinResponseTime { get; set; }
                public decimal MaxResponseTime { get; set; }
                public decimal AvgResponseTime { get; set; }

                public decimal SuccessMinResponseTime { get; set; }
                public decimal SuccessMaxResponseTime { get; set; }
                public decimal SuccessAvgResponseTime { get; set; }

                public decimal ErrorMinResponseTime { get; set; }
                public decimal ErrorMaxResponseTime { get; set; }
                public decimal ErrorAvgResponseTime { get; set; }

                public IDictionary<HttpStatusCode, int> StatusCodes { get; init; } = new Dictionary<HttpStatusCode, int>();

                public static TestResultGroupItem Map(TestResult.TestResultGroupItem item)
                {
                    return new TestResultGroupItem
                    {
                        Method = item.Method,
                        Host = item.Host,
                        Url = item.Url,
                        Query = item.Query,
                        AbsoluteUrl = item.AbsoluteUrl,
                        SuccessCount = item.SuccessCount,
                        ErrorCount = item.ErrorCount,
                        MinResponseTime = item.MinResponseTime,
                        MaxResponseTime = item.MaxResponseTime,
                        AvgResponseTime = item.AvgResponseTime,
                        SuccessMinResponseTime = item.SuccessMinResponseTime,
                        SuccessMaxResponseTime = item.SuccessMaxResponseTime,
                        SuccessAvgResponseTime = item.SuccessAvgResponseTime,
                        ErrorMinResponseTime = item.ErrorMinResponseTime,
                        ErrorMaxResponseTime = item.ErrorMaxResponseTime,
                        ErrorAvgResponseTime = item.ErrorAvgResponseTime,
                        StatusCodes = item.StatusCodes,
                    };
                }
            }
        }
    }
}

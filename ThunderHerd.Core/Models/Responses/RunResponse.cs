using ThunderHerd.Core.Enums;
using ThunderHerd.Core.Models.Dtos;

namespace ThunderHerd.Core.Models.Responses
{
    public partial class RunResponse
    {
        public DateTime RunStarted { get; set; }
        public DateTime RunCompleted { get; set; }
        public TimeSpan WarmupDuration { get; set; }
        public TimeSpan RunDuration { get; set; }
        public long NumTotalCalls { get; set; }
        public long NumSuccessCalls { get; set; }
        public long NumErrorCalls {  get; set; }

        public IEnumerable<TestResultItem> Results { get; set; } = new HashSet<TestResultItem>();

        public static RunResponse Map(RunResult result)
        {
            ArgumentNullException.ThrowIfNull(result, nameof(result));

            return new RunResponse
            {
                RunStarted = result.RunStarted,
                RunCompleted = result.RunCompleted,
                WarmupDuration = result.WarmupDuration,
                RunDuration = result.RunDuration,
                
                NumTotalCalls = result.NumTotalCalls,
                NumSuccessCalls = result.NumSuccessCalls,
                NumErrorCalls = result.NumErrorCalls,

                Results = result.Results.Count() > 0
                ? result.Results.Select(TestResultItem.Map).ToHashSet()
                : Enumerable.Empty<TestResultItem>(),
            };
        }
    }

    public partial class RunResponse
    {
        public class TestResultItem
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

            public static TestResultItem Map(RunResult.TestResultItem item)
            {
                return new TestResultItem
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
                };
            }
        }
    }
}

using ThunderHerd.Core.Enums;
using ThunderHerd.Core.Extensions;
using ThunderHerd.Core.Helpers;

namespace ThunderHerd.Core.Models.Dtos
{
    public partial class RunResult
    {
        public DateTime RunStarted { get; set; }
        public DateTime RunCompleted { get; set; }
        public TimeSpan RunDuration { get; set; }
        public TimeSpan WarmupDuration { get; set; }
        public long NumTotalCalls => NumSuccessCalls + NumErrorCalls;
        public long NumSuccessCalls => Results.Sum(p => p.SuccessCount);
        public long NumErrorCalls => Results.Sum(p => p.ErrorCount);
        public IEnumerable<TestResultItem> Results { get; set; } = new HashSet<TestResultItem>();
    }

    public partial class RunResult
    {
        public class TestResultItem
        {
            public HttpMethods Method { get; set; }
            public string? Host { get; set; }
            public string? Url { get; set; }
            public string? Query { get; set; }
            public string? AbsoluteUrl { get; set; }
            public int SuccessCount { get; set; }
            public int ErrorCount { get; set; }
            public long TotalCount => SuccessCount + ErrorCount;

            public decimal MinResponseTime { get; set; }
            public decimal MaxResponseTime { get; set; }
            public decimal AvgResponseTime { get; set; }

            public decimal SuccessMinResponseTime { get; set; }
            public decimal SuccessMaxResponseTime { get; set; }
            public decimal SuccessAvgResponseTime { get; set; }

            public decimal ErrorMinResponseTime { get; set; }
            public decimal ErrorMaxResponseTime { get; set; }
            public decimal ErrorAvgResponseTime { get; set; }

            public static TestResultItem Map(IGrouping<Uri?, HttpResponseMessage> group)
            {
                var headerName = Globals.HeaderNames.ElapsedTimeInMilliseconds;
                var uri = group.Key;

                return new TestResultItem
                {
                    Method = HttpMethods.GET,
                    Host = uri?.Host,
                    Url = uri?.AbsolutePath,
                    Query = uri?.Query,
                    AbsoluteUrl = uri?.AbsoluteUri,

                    SuccessCount = group.Count(e => e.IsSuccessStatusCode),
                    ErrorCount = group.Count(e => !e.IsSuccessStatusCode),

                    MinResponseTime = ItemHelper.Min(group, headerName).Round(),
                    MaxResponseTime = ItemHelper.Max(group, headerName).Round(),
                    AvgResponseTime = ItemHelper.Avg(group, headerName).Round(),

                    SuccessMinResponseTime = ItemHelper.Min(group.Where(p => p.IsSuccessStatusCode), headerName).Round(),
                    SuccessMaxResponseTime = ItemHelper.Max(group.Where(p => p.IsSuccessStatusCode), headerName).Round(),
                    SuccessAvgResponseTime = ItemHelper.Avg(group.Where(p => p.IsSuccessStatusCode), headerName).Round(),

                    ErrorMinResponseTime = ItemHelper.Min(group.Where(p => !p.IsSuccessStatusCode), headerName).Round(),
                    ErrorMaxResponseTime = ItemHelper.Max(group.Where(p => !p.IsSuccessStatusCode), headerName).Round(),
                    ErrorAvgResponseTime = ItemHelper.Avg(group.Where(p => !p.IsSuccessStatusCode), headerName).Round(),
                };
            }
        }
    }
}

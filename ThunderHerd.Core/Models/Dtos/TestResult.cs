using System.Net;
using ThunderHerd.Core.Enums;
using ThunderHerd.Core.Extensions;

namespace ThunderHerd.Core.Models.Dtos
{
    public partial class TestResult
    {
        public Guid Id { get; set; }
        public Guid TestId { get; set; }
        public DateTime RunStarted { get; set; }
        public DateTime RunCompleted { get; set; }
        public TimeSpan RunDuration { get; set; }
        public TimeSpan WarmupDuration { get; set; }
        public TestStatus Status { get; set; } = TestStatus.NA;

        public long NumTotalCalls => NumSuccessCalls + NumErrorCalls;
        public long NumSuccessCalls => TimeSlotCollection.Sum(p => p.ResultGroupCollection.Sum(e => e.SuccessCount));
        public long NumErrorCalls => TimeSlotCollection.Sum(p => p.ResultGroupCollection.Sum(e => e.ErrorCount));
        public IEnumerable<TestResultSlotItem> TimeSlotCollection { get; init; } = new HashSet<TestResultSlotItem>();

        public static TestResult? Map(Entities.TestResult? entity)
        {
            if (entity == default)
                return default;

            return new TestResult
            {
                Id = entity.Id,
                TestId = entity.TestId,
                RunStarted = entity.RunStarted,
                RunCompleted = entity.RunCompleted,
                RunDuration = entity.RunDuration,
                WarmupDuration = entity.WarmupDuration,
                Status = entity.Status,
            };
        }

        public class TestResultSlotItem
        {
            public long Tick { get; set; }
            public TimeSpan TimeSpan { get; set; }
            public IEnumerable<TestResultGroupItem> ResultGroupCollection { get; set; } = new HashSet<TestResultGroupItem>();

            public static TestResultSlotItem Map(IGrouping<long, HttpResponseMessage> group, TimeSpan timeSpan)
            {
                return new TestResultSlotItem
                {
                    Tick = long.Parse(group.First().RequestMessage.GetHeaderValue(Globals.HeaderNames.StartTimeInTicks)),
                    TimeSpan = timeSpan,
                    ResultGroupCollection = group
                        .GroupBy(p => p.RequestMessage?.RequestUri, p => p)
                        .Select(TestResultGroupItem.Map)
                        .ToHashSet()
                };
            }
        }

        public class TestResultGroupItem
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

            public IDictionary<HttpStatusCode, int> StatusCodes { get; init; } = new Dictionary<HttpStatusCode, int>();

            public static TestResultGroupItem Map(IGrouping<Uri?, HttpResponseMessage> group)
            {
                var headerName = Globals.HeaderNames.ElapsedTimeInMilliseconds;
                var uri = group.Key;

                var responseTimes = group.Select(p => p.RequestMessage);
                var successResponseTimes = group.Where(p => p.IsSuccessStatusCode).Select(p => p.RequestMessage);
                var errorResponseTimes = group.Where(p => !p.IsSuccessStatusCode).Select(p => p.RequestMessage);

                return new TestResultGroupItem
                {
                    Method = HttpMethods.GET,
                    Host = uri?.Host,
                    Url = uri?.AbsolutePath,
                    Query = uri?.Query,
                    AbsoluteUrl = uri?.AbsoluteUri,

                    SuccessCount = group.Count(e => e.IsSuccessStatusCode),
                    ErrorCount = group.Count(e => !e.IsSuccessStatusCode),

                    MinResponseTime = responseTimes.Min(headerName),
                    MaxResponseTime = responseTimes.Max(headerName),
                    AvgResponseTime = responseTimes.Avg(headerName),

                    SuccessMinResponseTime = successResponseTimes.Min(headerName),
                    SuccessMaxResponseTime = successResponseTimes.Max(headerName),
                    SuccessAvgResponseTime = successResponseTimes.Avg(headerName),

                    ErrorMinResponseTime = errorResponseTimes.Min(headerName),
                    ErrorMaxResponseTime = errorResponseTimes.Max(headerName),
                    ErrorAvgResponseTime = errorResponseTimes.Avg(headerName),

                    StatusCodes = group
                        .GroupBy(p => p.StatusCode, p => p)
                        .ToDictionary(p => p.Key, p => p.Count()),
                };
            }
        }
    }
}

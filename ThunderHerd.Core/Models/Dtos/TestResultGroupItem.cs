using ThunderHerd.Core.Enums;
using ThunderHerd.Core.Extensions;

namespace ThunderHerd.Core.Models.Dtos
{
    public class TestResultGroupItem
    {
        public HttpMethods Method { get; set; }
        public string Name { get; set; }
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


        public static TestResultGroupItem Map(IGrouping<string?, TestResultItem> group)
        {
            var entity = group.First();
            var successes = group.Where(p => !p.IsFaulty);
            var errors = group.Where(p => p.IsFaulty);
            var successCount = successes.Count();
            var errorCount = errors.Count();

            return new TestResultGroupItem
            {
                Method = entity.Method,
                Host = entity.Host,
                Url = entity.Url,
                Query = entity.Query,
                AbsoluteUrl = entity.AbsoluteUrl,

                SuccessCount = successCount,
                ErrorCount = errorCount,

                MinResponseTime = group.Min(p => p.ResponseTime).Round(),
                MaxResponseTime = group.Max(p => p.ResponseTime).Round(),
                AvgResponseTime = group.Average(p => p.ResponseTime).Round(),

                SuccessMinResponseTime = successCount > 0 ? successes.Min(p => p.ResponseTime).Round() : 0,
                SuccessMaxResponseTime = successCount > 0 ? successes.Max(p => p.ResponseTime).Round() : 0,
                SuccessAvgResponseTime = successCount > 0 ? successes.Average(p => p.ResponseTime).Round() : 0,

                ErrorMinResponseTime = errorCount > 0 ? errors.Min(p => p.ResponseTime).Round() : 0,
                ErrorMaxResponseTime = errorCount > 0 ? errors.Max(p => p.ResponseTime).Round() : 0,
                ErrorAvgResponseTime = errorCount > 0 ? errors.Average(p => p.ResponseTime).Round() : 0,
            };
        }
    }
}

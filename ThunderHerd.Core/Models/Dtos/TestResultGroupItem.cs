using System.Net.Sockets;
using ThunderHerd.Core.Enums;

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

        public static async Task<TestResultGroupItem> Map(IAsyncGrouping<string, TestResultItem> group)
        {
            var entity = await group.FirstOrDefaultAsync();
            var successes = group.Where(p => !p.IsFaulty);
            var errors = group.Where(p => p.IsFaulty);
            var successCount = successes.CountAsync();
            var errorCount = errors.CountAsync();

            return new TestResultGroupItem
            {
                Method = entity.Method,
                Host = entity.Host,
                Url = entity.Url,
                Query = entity.Query,
                AbsoluteUrl = entity.AbsoluteUrl,

                SuccessCount = await successCount,
                ErrorCount = await errorCount,

                MinResponseTime = await group.MinAsync(p => p.ResponseTime),
                MaxResponseTime = await group.MaxAsync(p => p.ResponseTime),
                AvgResponseTime = await group.AverageAsync(p => p.ResponseTime),

                SuccessMinResponseTime = await successes.MinAsync(p => p.ResponseTime),
                SuccessMaxResponseTime = await successes.MaxAsync(p => p.ResponseTime),
                SuccessAvgResponseTime = await successes.AverageAsync(p => p.ResponseTime),

                ErrorMinResponseTime = await errors.MinAsync(p => p.ResponseTime),
                ErrorMaxResponseTime = await errors.MaxAsync(p => p.ResponseTime),
                ErrorAvgResponseTime = await errors.AverageAsync(p => p.ResponseTime),
            };
        }
    }
}

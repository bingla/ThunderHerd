using ThunderHerd.Core.Enums;
using ThunderHerd.Core.Models.Dtos;

namespace ThunderHerd.Core.Models.Responses
{
    public class TestResultGroupItemResponse
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

        public static TestResultGroupItemResponse Map(TestResultGroupItem entity)
        {
            return new TestResultGroupItemResponse
            {
                Method = entity.Method,
                Host = entity.Host,
                Url = entity.Url,
                Query = entity.Query,
                AbsoluteUrl = entity.AbsoluteUrl,
                SuccessCount = entity.SuccessCount,
                ErrorCount = entity.ErrorCount,
                MinResponseTime = entity.MinResponseTime,
                MaxResponseTime = entity.MaxResponseTime,
                AvgResponseTime = entity.AvgResponseTime,
                SuccessMinResponseTime = entity.SuccessMinResponseTime,
                SuccessMaxResponseTime = entity.SuccessMaxResponseTime,
                SuccessAvgResponseTime = entity.SuccessAvgResponseTime,
                ErrorMinResponseTime = entity.ErrorMinResponseTime,
                ErrorMaxResponseTime = entity.ErrorMaxResponseTime,
                ErrorAvgResponseTime = entity.ErrorAvgResponseTime,
            };
        }
    }
}

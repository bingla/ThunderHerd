using System.Net;
using ThunderHerd.Core.Enums;

namespace ThunderHerd.Core.Entities
{
    public class TestResultItem
    {
        public long Id { get; set; }
        public Guid TestResultId { get; set; }
        public long Ticks { get; set; }
        public HttpMethods Method { get; set; }
        public string? Host { get; set; }
        public string? Url { get; set; }
        public string? Query { get; set; }
        public string? AbsoluteUrl { get; set; }
        public decimal ResponseTime { get; set; }
        public bool IsFaulty { get; set; } = false;
        public HttpStatusCode StatusCode { get; set; }

        public static TestResultItem Map(Models.Dtos.TestResultItem entity)
        {
            return new TestResultItem
            {
                Id = entity.Id,
                TestResultId = entity.TestResultId,
                Ticks = entity.Ticks,
                Method = entity.Method,
                Host = entity.Host,
                Query = entity.Query,
                AbsoluteUrl = entity.AbsoluteUrl,
                ResponseTime = entity.ResponseTime,
                IsFaulty = entity.IsFaulty,
                StatusCode = entity.StatusCode,
            };
        }
    }
}

using System.Net;
using ThunderHerd.Core.Enums;
using ThunderHerd.Core.Extensions;

namespace ThunderHerd.Core.Models.Dtos
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

        public static TestResultItem Map(Guid testResultId, HttpResponseMessage response)
        {
            var headerName = Globals.HeaderNames.ElapsedTimeInMilliseconds;
            var uri = response?.RequestMessage?.RequestUri ?? throw new ArgumentNullException(nameof(response.RequestMessage.RequestUri));

            var tick = response.RequestMessage.TryGetHeaderValue(Globals.HeaderNames.StartTimeInTicks, out var tickValue)
                ? long.Parse(tickValue?.FirstOrDefault() ?? "0")
                : 0;

            var responseTime = response.RequestMessage.TryGetHeaderValue(headerName, out var responseTimeValue)
                ? decimal.Parse(responseTimeValue?.FirstOrDefault() ?? "0").Round()
                : 0;

            return new TestResultItem()
            {
                TestResultId = testResultId,
                Ticks = tick,
                Method = HttpMethods.GET,
                Host = uri?.Host,
                Url = uri?.AbsolutePath,
                Query = uri?.Query,
                AbsoluteUrl = uri?.AbsoluteUri,
                ResponseTime = responseTime,
                IsFaulty = !response.IsSuccessStatusCode,
                StatusCode = response.StatusCode,
            };
        }

        public static TestResultItem Map(Entities.TestResultItem entity)
        {
            return new TestResultItem
            {
                Id = entity.Id,
                Ticks = entity.Ticks,
                TestResultId = entity.TestResultId,
                Method = entity.Method,
                Host = entity.Host,
                Url = entity.Url,
                Query = entity.Query,
                AbsoluteUrl = entity.AbsoluteUrl,
                ResponseTime = entity.ResponseTime,
                IsFaulty = entity.IsFaulty,
                StatusCode = entity.StatusCode,
            };
        }
    }
}

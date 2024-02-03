namespace ThunderHerd.Core.Helpers
{
    public static class ItemHelper
    {
        private static string _decimalZero = "0.0";

        public static decimal Min(IEnumerable<HttpResponseMessage> responseList, string headerName)
        {
            return responseList != default && responseList.Count() > 0
                ? responseList.Min(p => Decimal.Parse(GetHeaderValues(p, headerName) ?? _decimalZero))
                : 0;
        }

        public static decimal Max(IEnumerable<HttpResponseMessage> responseList, string headerName)
        {
            return responseList != default && responseList.Count() > 0
                ? responseList.Max(p => Decimal.Parse(GetHeaderValues(p, headerName) ?? _decimalZero))
                : 0;
        }

        public static decimal Avg(IEnumerable<HttpResponseMessage> responseList, string headerName)
        {
            return responseList != default && responseList.Count() > 0
                ? responseList.Average(p => Decimal.Parse(GetHeaderValues(p, headerName) ?? _decimalZero))
                : 0;
        }

        public static string? GetHeaderValues(HttpResponseMessage response, string headerName)
        {
            return GetHeaderValues(response?.RequestMessage, headerName);
        }

        public static string? GetHeaderValues(HttpRequestMessage? request, string headerName)
        {
            if (request?.Headers == default)
                return default;

            //return request?.Headers?.GetValues(headerName).FirstOrDefault();
            return request.Headers.TryGetValues(headerName, out var result)
                ? result?.FirstOrDefault()
                : null;
        }
    }
}

namespace ThunderHerd.Core.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public const decimal DecimalZero = 0;

        public static string? GetHeaderValue(this HttpRequestMessage? msg, string? header)
        {
            return msg.TryGetHeaderValue(header, out var value) ? value?.FirstOrDefault() : default;
        }

        public static bool TryGetHeaderValue(this HttpRequestMessage? msg, string? header, out IEnumerable<string>? result)
        {
            return msg.Headers.TryGetValues(header ?? string.Empty, out result);
        }

        public static decimal Min(this IEnumerable<HttpRequestMessage?>? msg, string? headerName, int precision = 4)
        {
            return msg != default && msg.Any(p => p != default)
                ? msg
                    .Where(p => p != default)
                    .Min(p => decimal.Parse(p.GetHeaderValue(headerName) ?? DecimalZero.ToString()))
                    .Round(precision)
                : DecimalZero;
        }

        public static decimal Max(this IEnumerable<HttpRequestMessage?>? msg, string? headerName, int precision = 4)
        {
            return msg != default && msg.Any(p => p != default)
                ? msg
                    .Where(p => p != default)
                    .Max(p => decimal.Parse(p.GetHeaderValue(headerName) ?? DecimalZero.ToString()))
                    .Round(precision)
                : DecimalZero;
        }

        public static decimal Avg(this IEnumerable<HttpRequestMessage?>? msg, string? headerName, int precision = 4)
        {
            return msg != default && msg.Any(p => p != default)
                ? msg
                    .Where(p => p != default)
                    .Average(p => decimal.Parse(p.GetHeaderValue(headerName) ?? DecimalZero.ToString()))
                    .Round(precision)
                : DecimalZero;
        }
    }
}

namespace ThunderHerd.Core.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static string? GetHeaderValue(this HttpResponseMessage? msg, string? header)
        {
            return msg.TryGetHeaderValue(header, out var value) ? value?.FirstOrDefault() : default;
        }

        public static bool TryGetHeaderValue(this HttpResponseMessage? msg, string? header, out IEnumerable<string>? result)
        {
            return msg.Headers.TryGetValues(header ?? string.Empty, out result);
        }
    }
}

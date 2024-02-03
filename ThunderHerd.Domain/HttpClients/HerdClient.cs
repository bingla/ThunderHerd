using ThunderHerd.Core.Enums;
using ThunderHerd.Domain.Interfaces;

namespace ThunderHerd.Domain.HttpClients
{
    public class HerdClient : IHerdClient
    {
        private readonly HttpClient _httpClient;

        public HerdClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<HttpResponseMessage> SendAsync(string url, HttpMethods method = HttpMethods.GET, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(url, nameof(url));
            return SendAsync(new Uri(url), method, cancellationToken);
        }

        public Task<HttpResponseMessage> SendAsync(Uri uri, HttpMethods method = HttpMethods.GET, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(uri, nameof(uri));
            var request = new HttpRequestMessage(ParseHttpMethod(method), uri);
            return _httpClient.SendAsync(request, cancellationToken);
        }

        private static HttpMethod ParseHttpMethod(HttpMethods method)
        {
            return method switch
            {
                HttpMethods.POST => HttpMethod.Post,
                HttpMethods.PUT => HttpMethod.Put,
                HttpMethods.DELETE => HttpMethod.Delete,
                _ => HttpMethod.Get,
            };
        }
    }
}

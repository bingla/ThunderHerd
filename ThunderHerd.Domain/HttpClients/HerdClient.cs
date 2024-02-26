using Microsoft.Extensions.Options;
using ThunderHerd.Core;
using ThunderHerd.Core.Enums;
using ThunderHerd.Core.Models.Settings;
using ThunderHerd.Core.Options;
using ThunderHerd.Domain.Interfaces;

namespace ThunderHerd.Domain.HttpClients
{
    public class HerdClient : IHerdClient
    {
        private readonly IOptions<TestServiceOptions> _options;
        private readonly HttpClient _httpClient;

        public HerdClient(IOptions<TestServiceOptions> options, HttpClient httpClient)
        {
            _options = options;
            _httpClient = httpClient;

            // Set the client timeout to double the max runtime duration
            // TODO: Setting the client timeout so high might degrade performance, look into how long a connection is considered
            // "open" when tasks are put on queue.
            _httpClient.Timeout = TimeSpan.FromMinutes(_options.Value.MaxRunDurationInSeconds * 2);
        }

        public Task<HttpResponseMessage> SendAsync(string url, HttpMethods method = HttpMethods.GET, HerdClientRequestSettings? settings = default, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(url, nameof(url));
            return SendAsync(new Uri(url), method, settings, cancellationToken);
        }

        public Task<HttpResponseMessage> SendAsync(Uri uri, HttpMethods method = HttpMethods.GET, HerdClientRequestSettings? settings = default, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(uri, nameof(uri));
            var request = SignRequest(new HttpRequestMessage(ParseHttpMethod(method), uri), settings);
            return _httpClient.SendAsync(request, cancellationToken);
        }

        private static HttpRequestMessage SignRequest(HttpRequestMessage request, HerdClientRequestSettings? settings = default)
        {
            request.Headers.Add(Globals.HeaderNames.StartTimeInTicks, DateTime.Now.Ticks.ToString());

            if (!string.IsNullOrEmpty(settings?.AppId) && !string.IsNullOrEmpty(settings?.AppSecret))
            {
                // TODO: Sign request with Auth Header
            }
            if (!string.IsNullOrEmpty(settings?.ApiKey))
            {
                request.Headers.Add(Globals.HeaderNames.XApiKey, settings.ApiKey);
            }

            return request;
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

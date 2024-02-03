using ThunderHerd.Core.Enums;

namespace ThunderHerd.Domain.Interfaces
{
    public interface IHerdClient
    {
        Task<HttpResponseMessage> SendAsync(string url, HttpMethods method = HttpMethods.GET, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> SendAsync(Uri uri, HttpMethods method = HttpMethods.GET, CancellationToken cancellationToken = default);
    }
}

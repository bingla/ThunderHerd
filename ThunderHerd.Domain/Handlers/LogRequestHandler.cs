using System.Diagnostics;
using ThunderHerd.Core;

namespace ThunderHerd.Domain.Handlers
{
    public class LogRequestHandler : DelegatingHandler
    {
        public LogRequestHandler()
        {

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            var response = await base.SendAsync(request, cancellationToken);

            request.Headers.Add(Globals.HeaderNames.ElapsedTimeInMilliseconds,
                stopwatch.Elapsed.TotalMilliseconds.ToString());

            return response;
        }
    }
}

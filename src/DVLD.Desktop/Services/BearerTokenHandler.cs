using System.Net.Http.Headers;
using DVLD.Desktop.Common;

namespace DVLD.Desktop.Services
{
    /// <summary>
    /// Adds the current session token to each request so re-login cannot leave a stale bearer token behind.
    /// </summary>
    internal sealed class BearerTokenHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(SessionManager.Token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.Token);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}

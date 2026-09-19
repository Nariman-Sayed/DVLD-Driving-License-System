using System.Net.Http.Headers;

namespace DVLD.Desktop.Common
{
    public static class ApiClientFactory
    {
        public static HttpClient CreateAuthorizedClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ApiSettings.BaseUrl);

            if (!string.IsNullOrEmpty(SessionManager.Token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", SessionManager.Token);
            }

            return client;
        }
    }
}
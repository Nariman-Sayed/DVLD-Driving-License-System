using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DVLD.Desktop.Common;

namespace DVLD.Desktop.Services
{
    /// <summary>
    /// The single long-lived HTTP client used by the desktop application.
    /// </summary>
    public sealed class ApiClient
    {
        private static readonly Lazy<ApiClient> LazyInstance = new(() => new ApiClient());
        private readonly HttpClient _httpClient;

        private ApiClient()
        {
            var configuredBaseUrl = DVLD.Properties.Settings.Default.ApiBaseUrl;

            if (string.IsNullOrWhiteSpace(configuredBaseUrl))
            {
                throw new InvalidOperationException(
                    "The API base URL is not configured. Set ApiBaseUrl in the application settings.");
            }

            var normalizedBaseUrl = configuredBaseUrl.Trim();
            if (!normalizedBaseUrl.EndsWith('/'))
            {
                normalizedBaseUrl += "/";
            }

            if (!Uri.TryCreate(normalizedBaseUrl, UriKind.Absolute, out var baseUri)
                || (baseUri.Scheme != Uri.UriSchemeHttp && baseUri.Scheme != Uri.UriSchemeHttps))
            {
                throw new InvalidOperationException(
                    "The API base URL in the application settings is not a valid HTTP or HTTPS address.");
            }

            _httpClient = new HttpClient(new BearerTokenHandler
            {
                InnerHandler = new HttpClientHandler()
            })
            {
                BaseAddress = baseUri,
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        /// <summary>
        /// Gets the process-wide client instance.
        /// </summary>
        public static ApiClient Instance => LazyInstance.Value;

        /// <summary>
        /// Raised after an authenticated request is rejected with HTTP 401 and the session was cleared.
        /// </summary>
        public event EventHandler? Unauthorized;

        /// <summary>
        /// Posts a JSON body and reads a JSON response.
        /// </summary>
        /// <typeparam name="TRequest">The request body type.</typeparam>
        /// <typeparam name="TResponse">The response body type.</typeparam>
        /// <param name="relativePath">The path relative to the configured base URL.</param>
        /// <param name="body">The request body.</param>
        /// <param name="cancellationToken">A token to cancel the request.</param>
        /// <returns>The deserialized response, or <see langword="null"/> for an empty body.</returns>
        /// <exception cref="ApiException">The API rejected the request or could not be reached.</exception>
        public Task<TResponse?> PostAsync<TRequest, TResponse>(
            string relativePath,
            TRequest body,
            CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, relativePath)
            {
                Content = JsonContent.Create(body)
            };

            return SendForJsonAsync<TResponse>(request, cancellationToken);
        }

        /// <summary>
        /// Sends a GET request and reads a JSON response.
        /// </summary>
        /// <typeparam name="TResponse">The response body type.</typeparam>
        /// <param name="relativePath">The path relative to the configured base URL.</param>
        /// <param name="cancellationToken">A token to cancel the request.</param>
        /// <returns>The deserialized response, or <see langword="null"/> for an empty body.</returns>
        /// <exception cref="ApiException">The API rejected the request or could not be reached.</exception>
        public Task<TResponse?> GetAsync<TResponse>(
            string relativePath,
            CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, relativePath);
            return SendForJsonAsync<TResponse>(request, cancellationToken);
        }

        /// <summary>
        /// Sends a GET request and returns the raw response body.
        /// </summary>
        /// <param name="relativePath">The path relative to the configured base URL.</param>
        /// <param name="cancellationToken">A token to cancel the request.</param>
        /// <returns>The response body as text.</returns>
        /// <exception cref="ApiException">The API rejected the request or could not be reached.</exception>
        public async Task<string> GetStringAsync(
            string relativePath,
            CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, relativePath);
            using var response = await SendAsync(request, cancellationToken);
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }

        private async Task<TResponse?> SendForJsonAsync<TResponse>(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            using (request)
            using (var response = await SendAsync(request, cancellationToken))
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return default;
                }

                try
                {
                    return await response.Content.ReadFromJsonAsync<TResponse>(
                        cancellationToken: cancellationToken);
                }
                catch (JsonException ex)
                {
                    throw new ApiException(
                        "The DVLD API returned a response that could not be read.",
                        response.StatusCode,
                        ex);
                }
                catch (NotSupportedException ex)
                {
                    throw new ApiException(
                        "The DVLD API returned an unsupported response format.",
                        response.StatusCode,
                        ex);
                }
            }
        }

        private async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage response;

            try
            {
                response = await _httpClient.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken);
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                throw new ApiException("The request to the DVLD API timed out. Please try again.");
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException(
                    $"Cannot reach the DVLD API at {_httpClient.BaseAddress}. "
                    + "Make sure the API is running and the configured URL is correct.",
                    innerException: ex);
            }

            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            using (response)
            {
                var message = await ReadErrorMessageAsync(response, cancellationToken);
                var statusCode = response.StatusCode;

                if (statusCode == HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                }

                throw new ApiException(message, statusCode);
            }
        }

        private void HandleUnauthorized()
        {
            var hadSession = SessionManager.IsLoggedIn;
            SessionManager.ClearSession();

            if (hadSession)
            {
                Unauthorized?.Invoke(this, EventArgs.Empty);
            }
        }

        private static async Task<string> ReadErrorMessageAsync(
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            string body;

            try
            {
                body = await response.Content.ReadAsStringAsync(cancellationToken);
            }
            catch (Exception ex) when (ex is HttpRequestException or IOException)
            {
                body = string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(body))
            {
                try
                {
                    using var document = JsonDocument.Parse(body);
                    var root = document.RootElement;

                    if (root.ValueKind == JsonValueKind.Object)
                    {
                        var validationErrors = ReadValidationErrors(root);
                        if (validationErrors.Count > 0)
                        {
                            return string.Join(Environment.NewLine, validationErrors);
                        }

                        if (TryGetNonEmptyString(root, "detail", out var detail))
                        {
                            return detail;
                        }

                        if (TryGetNonEmptyString(root, "title", out var title))
                        {
                            return title;
                        }

                        if (TryGetNonEmptyString(root, "message", out var message))
                        {
                            return message;
                        }
                    }
                }
                catch (JsonException)
                {
                    // Not a ProblemDetails/JSON body; use the generic status message below.
                }
            }

            return $"The DVLD API returned an error ({(int)response.StatusCode} {response.ReasonPhrase}).";
        }

        private static List<string> ReadValidationErrors(JsonElement root)
        {
            var messages = new List<string>();

            if (!root.TryGetProperty("errors", out var errors)
                || errors.ValueKind != JsonValueKind.Object)
            {
                return messages;
            }

            foreach (var field in errors.EnumerateObject())
            {
                if (field.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in field.Value.EnumerateArray())
                    {
                        AddIfNotEmpty(messages, item);
                    }
                }
                else
                {
                    AddIfNotEmpty(messages, field.Value);
                }
            }

            return messages;
        }

        private static void AddIfNotEmpty(List<string> messages, JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.String)
            {
                var text = element.GetString();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    messages.Add(text);
                }
            }
        }

        private static bool TryGetNonEmptyString(JsonElement element, string propertyName, out string value)
        {
            if (element.TryGetProperty(propertyName, out var property)
                && property.ValueKind == JsonValueKind.String)
            {
                var text = property.GetString();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    value = text;
                    return true;
                }
            }

            value = string.Empty;
            return false;
        }
    }
}

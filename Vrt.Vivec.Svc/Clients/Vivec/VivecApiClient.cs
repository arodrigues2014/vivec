using Azure;
using System.Net;

namespace Vrt.Vivec.Svc.Clients.Vivec
{
    public class VivecApiClient : HttpClient
    {
        private readonly IConfiguration _configuration;

        public VivecApiClient(IConfiguration configuration)
        {
            _configuration = configuration;

            BaseAddress = new Uri(_configuration.GetValue<string>("Vivec:BaseUrl"));

            Timeout = new TimeSpan(0, 10, 0);
        }

        public async Task<string> SendRequest(HttpRequestMessage request)
        {
            try
            {
                var response = await SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    LogErrorAndThrow(statusCode: response.StatusCode, errorResponse: errorResponse, ex: null);
                    return errorResponse;

                }
            }
            catch (HttpRequestException ex)
            {
                LogErrorAndThrow(ex: ex);
            }
            catch (Exception ex)
            {
                LogErrorAndThrow(ex: ex);
            }

            return null; 
        }

        private void LogErrorAndThrow(HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string? errorResponse = null, Exception? ex = null )
        {
            string? errorMessage = ex != null ? $"HTTP Request Error: {ex.Message}" : errorResponse;

            if (statusCode != HttpStatusCode.InternalServerError || !string.IsNullOrEmpty(errorResponse))
            {
                Log.Logger.ForContext("Process", "SendRequest").Error($"HTTP Request Error. Status Code: {statusCode}, Response: {errorResponse}");
            }
        }
    }
}

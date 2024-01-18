
namespace Vrt.Vivec.Svc.Clients.Vivec;

public class VivecApiClient : HttpClient
{
    private readonly IConfiguration _configuration;

    public VivecApiClient(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        InitializeApiClient();
    }

    private void InitializeApiClient()
    {
        // Obtener la dirección base desde la configuración
        string baseUrl = _configuration.GetValue<string>("Vivec:BaseUrl");

        // Validar que la dirección base no sea nula o vacía
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException("The base address in the configuration is null or empty.");
        }

        // Configurar la dirección base y el tiempo de espera
        BaseAddress = new Uri(baseUrl);
        Timeout = TimeSpan.FromMinutes(10); // 10 minutos de tiempo de espera
    }

    public async Task<string> SendRequest(HttpRequestMessage request)
    {
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Or<Exception>()  
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        try
        {
            return await retryPolicy.ExecuteAsync(async () =>
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

            });
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

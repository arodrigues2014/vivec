
using Vrt.Vivec.Svc.Data.Response;
using Vrt.Vivec.Svc.Exceptions;

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

    public async Task<object> SendRequest(HttpRequestMessage request)
    {

        DialengaErrorDTO errorResponse = new DialengaErrorDTO();

        try
        {

            var response = await SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
              
                string jsonString = await response.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    Converters = { new EpochDateTimeConverterHelper() }
                };

                NewsDTO newsDTO = JsonConvert.DeserializeObject<NewsDTO>(jsonString, settings);

                foreach (var message in newsDTO.Messages)
                {
                    var html = message.Text;

                    // Crear un objeto HtmlDocument
                    HtmlDocument doc = new HtmlDocument();

                    // Cargar el HTML en el objeto HtmlDocument
                    doc.LoadHtml(html);

                    // Obtener el contenido completo del HTML utilizando InnerText
                    string innerText = doc.DocumentNode.InnerText;

                    message.Text = innerText;
                }

                return newsDTO;
               
            }
            else
            {
                string jsonString = await response.Content.ReadAsStringAsync();

                errorResponse = JsonConvert.DeserializeObject<DialengaErrorDTO>(jsonString);

                LogErrorAndThrow(statusCode: response.StatusCode, errorResponse: jsonString, ex: null);

                throw new ApiVivecException(statusCode: response.StatusCode, errorResponse: errorResponse, message: "Error occurred during HTTP request.");
            }
        }
        catch (ApiVivecException ex)
        {
            return errorResponse;
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

    public async Task<object> SendLoginRequest(HttpRequestMessage request)
    {

        DialengaErrorDTO errorResponse = new DialengaErrorDTO();
        TokenResultDTO bearerDTO = new TokenResultDTO();

        try
        {

            var response = await SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                bearerDTO = ExtractBearerToken(response);

                if (bearerDTO != null)
                {
                    return bearerDTO;
                }
            }
            else
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                errorResponse = JsonConvert.DeserializeObject<DialengaErrorDTO>(jsonString);
                LogErrorAndThrow(statusCode: response.StatusCode, errorResponse: jsonString, ex: null);
                throw new ApiVivecException(statusCode: response.StatusCode, errorResponse: errorResponse, message: "Error occurred during HTTP request.");
            }
        }
        catch (ApiVivecException ex)
        {
            return errorResponse;
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

    private TokenResultDTO ExtractBearerToken(HttpResponseMessage response)
    {
        var bearerDTO = new TokenResultDTO();

        if (response.Headers.TryGetValues("Authorization", out var authorizationHeaders))
        {
            foreach (var authorizationHeader in authorizationHeaders)
            {
                bearerDTO.AccessToken = authorizationHeader.Replace("Bearer ", "");
            }
        }

        return string.IsNullOrEmpty(bearerDTO.AccessToken) ? null : bearerDTO;
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

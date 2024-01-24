
using System.IdentityModel.Tokens.Jwt;
using Vrt.Vivec.Svc.Exceptions;

namespace Vrt.Vivec.Svc.Clients.Vivec;


public sealed class VivecApiClient : HttpClient
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private string _token;
    private string BaseUrl;
    private DateTime _expirationDate;

    private static readonly VivecApiClient instancia = new VivecApiClient();

    // Pasar IConfiguration como parámetro al constructor
    private VivecApiClient()
    {
        _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        BaseUrl = _configuration.GetValue<string>("Vivec:BaseUrl");
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl),
            Timeout = TimeSpan.FromMinutes(10)
        };
    }

    public static VivecApiClient Instancia => instancia;

    public async Task<object> ObtenerTokenAsync(HttpRequestMessage request)
    {
        DialengaErrorDTO errorResponse = new DialengaErrorDTO();
        TokenResultDTO bearerDTO = new TokenResultDTO();

        try
        {
            DateTime currentDate = DateTime.UtcNow;

            if (_expirationDate <= currentDate)
            {
                _token = string.Empty;
            }

            // Verificar si ya se ha obtenido un token
            if (string.IsNullOrEmpty(_token))
            {

                // Realizar la llamada POST al endpoint de login
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    bearerDTO = ExtractBearerToken(response);

                    if (bearerDTO != null)
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();

                        var jwtToken = tokenHandler.ReadToken(bearerDTO.AccessToken) as JwtSecurityToken;

                        if (jwtToken != null)
                        {
                            _expirationDate = jwtToken.ValidTo;
                        }

                        _token = bearerDTO.AccessToken;

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
        }
        catch (ApiVivecException ex)
        {
            // Manejar la excepción ApiVivec
            return errorResponse;
        }
        catch (HttpRequestException ex)
        {
            // Manejar la excepción HttpRequest
            LogErrorAndThrow(ex: ex);
        }
        catch (Exception ex)
        {
            // Manejar otras excepciones
            LogErrorAndThrow(ex: ex);
        }

        return _token;
    }

    public async Task<object> ObtenerNewsAsync(HttpRequestMessage request)
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

                var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

                var news = mapper.Map<NewsDTO, NewsHtmlDTO>(newsDTO);

                return news;
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
    
    private void LogErrorAndThrow(HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string? errorResponse = null, Exception? ex = null)
    {
        string? errorMessage = ex != null ? $"HTTP Request Error: {ex.Message}" : errorResponse;

        if (statusCode != HttpStatusCode.InternalServerError || !string.IsNullOrEmpty(errorResponse))
        {
            Log.Logger.ForContext("Process", "SendRequest").Error($"HTTP Request Error. Status Code: {statusCode}, Response: {errorResponse}");
        }
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
    public string Token => _token;
}

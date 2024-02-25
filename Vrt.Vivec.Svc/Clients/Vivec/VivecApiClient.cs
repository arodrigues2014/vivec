namespace Vrt.Vivec.Svc.Clients.Vivec;


public sealed class VivecApiClient : HttpClient
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    public string _token;
    private string _baseUrl;
    public DateTime _expirationDate;

    private static readonly VivecApiClient instancia = new VivecApiClient();

    private VivecApiClient()
    {
        _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        _baseUrl = _configuration.GetValue<string>("Vivec:BaseUrl");
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_baseUrl),
            Timeout = TimeSpan.FromMinutes(10)
        };
    }

    public string Token => _token;
    public static VivecApiClient Instancia => instancia;

    private async Task<object> GetTokenAsync(HttpRequestMessage request)
    {
        DialengaErrorDTO errorResponse = new DialengaErrorDTO();
        TokenResultDTO bearerDTO = new TokenResultDTO();

        try
        {
          
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
                //LogError(statusCode: response.StatusCode, errorResponse: jsonString, ex: null);
                throw new ApiVivecException(statusCode: response.StatusCode, errorResponse: errorResponse, message: "Error occurred during HTTP request.");
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
            //LogError(ex: ex);
            return errorResponse.Error = ex.Message;
        }
        catch (Exception ex)
        {
            // Manejar otras excepciones
            //LogError(ex: ex);
            return errorResponse.Error = ex.Message;
        }

        return null;
    }

    public async Task<object> GetNewsAsync(int page)
    {
        DialengaErrorDTO errorResponse = new DialengaErrorDTO();

        try
        {           

            DateTime currentDate = DateTime.UtcNow;

            if (_expirationDate <= currentDate)
            {
              
                var response = await GetTokenAsync(ConfigurationHelper.VivecPostLoginRequest("login"));
                if (response != null)
                {
                    Type resultObjectType = response?.GetType();

                    if (resultObjectType == typeof(DialengaErrorDTO))
                    {
                        return (DialengaErrorDTO)response;
                    }

                }
            }

            var request = ConfigurationHelper.VivecPostNewsRequest("Inbox", page, Token);

            if (request != null)
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

                    //LogError(statusCode: response.StatusCode, errorResponse: jsonString, ex: null);

                    throw new ApiVivecException(statusCode: response.StatusCode, errorResponse: errorResponse, message: "Error occurred during HTTP request.");
                }
            }
        }
        catch (ApiVivecException ex)
        {
            return errorResponse;
        }
        catch (HttpRequestException ex)
        {
            //LogError(ex: ex);
            return errorResponse.Error = ex.Message;
        }
        catch (Exception ex)
        {
            //LogError(ex: ex);
            return errorResponse.Error= ex.Message;
        }

        return null;
    }

    //private void LogError(HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string? errorResponse = null, Exception? ex = null)
    //{
    //    string? errorMessage = ex != null ? $"HTTP Request Error: {ex.Message}" : errorResponse;

    //    Log.Logger.ForContext("Process", "SendRequest").Error($"HTTP Request Error. Status Code: " + errorMessage);
    //}

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
}

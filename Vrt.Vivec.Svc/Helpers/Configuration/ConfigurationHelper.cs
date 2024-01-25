
using Newtonsoft.Json.Linq;
using System.Text;

namespace Vrt.Vivec.Svc.Helpers.Configuration;

public static class ConfigurationHelper
{
    private static IConfiguration _configuration= new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    private static string? BaseUrl => _configuration.GetValue<string>("Vivec:BaseUrl");
    private static string? endpointUrl;
    private static string? Page => _configuration.GetValue<string>("Vivec:Page");
    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

    }

    public static HttpRequestMessage VivecPostNewsRequest(string endpoint,int page,string token)
    {
        ValidateConfigurationAndEndpoint(endpoint);

        endpointUrl = _configuration.GetValue<string>($"Vivec:Endpoints:{endpoint}");

        string fullUrl = $"{BaseUrl}{endpointUrl}";

        var jsonBody = $"{{\"page\": {page}}}";

        HttpRequestMessage hrm = new HttpRequestMessage(HttpMethod.Post, fullUrl);

        // Configurar la solicitud POST con contenido JSON
        hrm.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        
        hrm.Headers.Add("Authorization", $"Bearer {token}");

        return hrm;
    }

    public static HttpRequestMessage VivecPostLoginRequest(string endpoint)
    {
        ValidateConfigurationAndEndpoint(endpoint);

        // Nombre de usuario y contraseña para la autorización 
        var username = _configuration?.GetValue<string>("Vivec:Username");
        var password = _configuration?.GetValue<string>("Vivec:Password");

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("Username or Password configuration is empty.");

        // Crear datos del formulario
        var formData = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password)
        };

        endpointUrl = _configuration.GetValue<string>($"Vivec:Endpoints:{endpoint}");

        string fullUrl = $"{BaseUrl}{endpointUrl}";

        HttpRequestMessage hrm = new HttpRequestMessage(HttpMethod.Post, fullUrl);

        // Configurar la solicitud POST con x-www-form-urlencoded
        hrm.Content = new FormUrlEncodedContent(formData);

        return hrm;
    }

    private static void ValidateConfigurationAndEndpoint(string endpoint)
    {
        if (_configuration == null)
            throw new InvalidOperationException("Configuration not initialized. Call Initialize before using this helper.");
 
        if (string.IsNullOrEmpty(endpoint))
            throw new InvalidOperationException("Endpoint configuration is missing or invalid.");

        endpointUrl = _configuration.GetValue<string>($"Vivec:Endpoints:{endpoint}");

        if (string.IsNullOrWhiteSpace(BaseUrl) || string.IsNullOrWhiteSpace(endpointUrl))
            throw new InvalidOperationException("BaseUrl or Endpoint configuration is missing or invalid.");
    }
}
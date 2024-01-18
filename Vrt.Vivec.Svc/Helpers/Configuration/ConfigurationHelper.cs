

namespace Vrt.Vivec.Svc.Helpers.Configuration;

public static class ConfigurationHelper
{
    private static IConfiguration _configuration;

    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public static HttpRequestMessage VivecPostLoginRequest(string endpoint)
    {
        if (_configuration == null)
        {
            throw new InvalidOperationException("Configuration not initialized. Call Initialize before using this helper.");
        }

        string? baseUrl = _configuration.GetValue<string>("Vivec:BaseUrl");

        string? endpointUrl = _configuration.GetValue<string>($"Vivec:Endpoints:{endpoint}");

        if (string.IsNullOrWhiteSpace(baseUrl) || string.IsNullOrWhiteSpace(endpointUrl))
        {
            throw new InvalidOperationException("BaseUrl or Endpoint configuration is missing or invalid.");
        }

        // Nombre de usuario y contraseña para la autorización 
        var username = _configuration?.GetValue<string>("Vivec:Username");
        var password = _configuration?.GetValue<string>("Vivec:Password");

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException("Username or Password configuration is empty.");
        }

        // Crear datos del formulario
        var formData = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password)
        };

        // Configurar la solicitud POST con x-www-form-urlencoded
        var content = new FormUrlEncodedContent(formData);

        string fullUrl = $"{baseUrl}{endpointUrl}";

        HttpRequestMessage hrm = new HttpRequestMessage(HttpMethod.Post, fullUrl);

        hrm.Content = content;

        return hrm;
    }
}
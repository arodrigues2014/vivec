namespace Vrt.Vivec.Svc.Helpers;

public static class SettingsHelper
{
    private static IConfiguration? _configuration;

    public static void Initialize(IConfiguration configuration) => _configuration = configuration;

    public static string? SwaggerInfoApiTitle => _configuration!["Swagger:InfoApi:Title"];
    public static string? SwaggerInfoApiDescription => _configuration!["Swagger:InfoApi:Description"];
    public static string? SwaggerInfoApiDeprecated => _configuration!["Swagger:InfoApi:DeprecatedDescription"];
    public static string? SwaggerInfoApiContactName => _configuration!["Swagger:InfoApi:Contact:Name"];
    public static string? SwaggerInfoApiContactEmail => _configuration!["Swagger:InfoApi:Contact:Email"];
    public static string? SwaggerInfoApiContactUrl => _configuration!["Swagger:InfoApi:Contact:Url"];
    public static string? SwaggerAuthenticationCliendId => _configuration!["Swagger:Authentication:ClientId"];
    public static string? SwaggerAuthenticationAppName => _configuration!["Swagger:Authentication:AppName"];
}

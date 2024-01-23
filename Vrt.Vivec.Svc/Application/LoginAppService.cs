namespace Vrt.Vivec.Svc.Application;

public interface ILoginAppService
{
    Task<IActionResult> LoginAsync();
}

// Implementación del servicio de aplicación
public class LoginAppService : ILoginAppService
{
    private readonly IConfiguration _configuration;
    private VivecApiClient? _client;

    public LoginAppService(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    private VivecApiClient GetClient()
    {
        return new VivecApiClient(_configuration);
    }

    public async Task<IActionResult> LoginAsync()
    {
        try
        {
            using (_client = GetClient())
            {
                ConfigurationHelper.Initialize(_configuration);

                var resultObject = await _client?.SendLoginRequest(ConfigurationHelper.VivecPostLoginRequest("Login"));

                IActionResult result = resultObject switch
                {
                    DialengaErrorDTO _ => new OkObjectResult(resultObject),
                    TokenResultDTO bearer => new OkObjectResult(resultObject),
                    _ => new OkObjectResult(null),
                };

                return result;
            }
        }
        catch (Exception ex)
        {
            Log.Logger.ForContext("Process", "Login").Error(ex, "Exception thrown getting data from Vivec");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

}

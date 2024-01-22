
namespace Vrt.Vivec.Svc.Application;

public interface ILoginAppService
{
    Task<IActionResult> LoginAsync(bool conductor);
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

    public async Task<IActionResult> LoginAsync(bool conductor)
    {
        try
        {
            
            using (_client = GetClient())
            {
                ConfigurationHelper.Initialize(_configuration);

                var resultObject = await _client?.SendRequest(ConfigurationHelper.VivecPostLoginRequest("Login"));

                var result = resultObject switch
                {
                    DialengaErrorDTO _ => new OkObjectResult(resultObject),
                    _ => resultObject is UserDTO ssoConfig
                        ? SsoConfig(ssoConfig, conductor)
                          : new OkObjectResult(false),
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

    Func<UserDTO, bool,  IActionResult> SsoConfig = (ssoConfig, conductor) =>
    {
        var result = ssoConfig?.company?.configuration?.loginConfiguration?.ssoConfigs?.ToList();
        if ((result.Any()))
        {
            int x = 0;
            if (!conductor)
                x = 1;

            LoginDTO loginDTO = new LoginDTO
            {
                loginURL = result[x].loginURL,
                clientSecret = result[x].clientSecret,
                clientId = result[x].clientId
            };

            // Return OkObjectResult with the evaluated ssoConfig
            return new OkObjectResult(loginDTO);
        }
        else
        {
            // Return OkObjectResult with false if the condition is not met
            return new OkObjectResult(false);
        }
    };

}

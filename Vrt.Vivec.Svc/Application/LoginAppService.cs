

using Vrt.Vivec.Svc.Validators;

namespace Vrt.Vivec.Svc.Application;

public interface ILoginAppService
{
    Task<IActionResult> LoginAsync(Usuario usuario);
}

// Implementación del servicio de aplicación
public class LoginAppService : ILoginAppService
{
    private readonly IUsuarioValidator _usuarioValidator;
    private readonly IConfiguration _configuration;
    private VivecApiClient? _client;

    public LoginAppService(IUsuarioValidator usuarioValidator, IConfiguration configuration)
    {
        _usuarioValidator = usuarioValidator ?? throw new ArgumentNullException(nameof(usuarioValidator));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    private VivecApiClient GetClient()
    {
        return new VivecApiClient(_configuration);
    }

    public async Task<IActionResult> LoginAsync(Usuario usuario)
    {
        try
        {
            var validationResult = _usuarioValidator.Validate(usuario);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(new ErrorResultHelper(usuario, validationResult.Errors));
            }

            using (_client = GetClient())
            {
                ConfigurationHelper.Initialize(_configuration);

                var resultObject = await _client?.SendRequest(ConfigurationHelper.VivecPostLoginRequest("Login"));

                return resultObject switch
                {
                    DialengaErrorDTO _ => new OkObjectResult(resultObject),
                    _ => new OkObjectResult(resultObject ?? false),
                };
            }
        }
        catch (Exception ex)
        {
            Log.Logger.ForContext("Process", "Login").Error(ex, "Exception thrown getting data from Vivec");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}

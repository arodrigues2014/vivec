

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

                //_client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}")

                var result = await _client?.SendRequest(ConfigurationHelper.VivecPostLoginRequest("Login"));

                return new OkObjectResult(result != null ? result : false);
            }
        }
        catch (Exception ex)
        {
            Log.Logger.ForContext("Process", "Login").Error(ex, "Exception thrown getting data from Vivec");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}

namespace Vrt.Vivec.Svc.Application;

public interface INewsAppService
{
    Task<IActionResult> NewsAsync(string token);
}

public class NewsAppService : INewsAppService
{
    private readonly IConfiguration _configuration;
    private VivecApiClient? _client;

    public NewsAppService(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    private VivecApiClient GetClient()
    {
        return new VivecApiClient(_configuration);
    }

    public async Task<IActionResult> NewsAsync(string token)
    {
        if (token == null || string.IsNullOrEmpty(token))
        {
            throw new InvalidOperationException("Token is null or empty.");
        }

        try
        {
            using (_client = GetClient())
            {
                ConfigurationHelper.Initialize(_configuration);

                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var resultObject = await _client?.SendRequest(ConfigurationHelper.VivecPostNewsRequest("Inbox"));

                IActionResult result = resultObject switch
                {
                    DialengaErrorDTO _ => new OkObjectResult(resultObject),
                    NewsHtmlDTO news => new OkObjectResult(resultObject),
                    _ => new OkObjectResult(null),
                };

                return result;
            }
        }
        catch (Exception ex)
        {
            Log.Logger.ForContext("Process", "Inbox").Error(ex, "Exception thrown getting data from Vivec");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        return null;
    }
}

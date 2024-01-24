namespace Vrt.Vivec.Svc.Application;

public interface INewsAppService
{
    Task<IActionResult> NewsAsync(int page);
}

public class NewsAppService : INewsAppService
{

    public async Task<IActionResult> NewsAsync(int page)
    {
        string token = string.Empty;

        if (page < 0)
        {
            throw new InvalidOperationException("Page error.");
        }

        try
        {
            var cliente = VivecApiClient.Instancia;

            var resultObject = await cliente.ObtenerTokenAsync(ConfigurationHelper.VivecPostLoginRequest("login"));

            var _token = string.Empty;

            Type resultObjectType = resultObject?.GetType();

            string resultObjectTypeName = resultObjectType?.FullName;

            if (resultObjectType == typeof(DialengaErrorDTO))
            {
                return new OkObjectResult(resultObject);
            }

            if (resultObjectType == typeof(TokenResultDTO))
            {
                var tokenObj = (TokenResultDTO)resultObject;
                _token = tokenObj.AccessToken;
            }
            else
            {
                _token = resultObject.ToString();
            }

            var resultObjectNews = await cliente.ObtenerNewsAsync(ConfigurationHelper.VivecPostNewsRequest("Inbox", page, _token));

            Type resultType = resultObjectNews?.GetType();

            if (resultObjectType == typeof(DialengaErrorDTO))
            {
                return new OkObjectResult(resultObjectNews);
            }

            return new OkObjectResult(resultObjectNews);
        }
        catch (HttpRequestException ex)
        {
            Log.Logger.ForContext("Process", "Inbox").Error(ex, "HttpRequestException thrown getting data from Vivec");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        catch (InvalidOperationException ex)
        {
            Log.Logger.ForContext("Process", "Inbox").Error(ex, "Invalid page number");
            return new BadRequestResult();
        }
        catch (Exception ex)
        {
            Log.Logger.ForContext("Process", "Inbox").Error(ex, "Exception thrown getting data from Vivec");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        return new OkObjectResult(null);
    }
}

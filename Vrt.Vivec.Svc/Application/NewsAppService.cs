namespace Vrt.Vivec.Svc.Application;

public interface INewsAppService
{
    Task<IActionResult> NewsAsync(int page);
}

public class NewsAppService : INewsAppService
{
    public async Task<IActionResult> NewsAsync(int page)
    {
        DialengaErrorDTO _error = new DialengaErrorDTO
        {
            Error = "Invalid page number",
            LocalizedError = "NewsAsync"
        };

        if (page < 0)
            return new BadRequestObjectResult(_error);

        try
        {
            var cliente = VivecApiClient.Instancia;
            var newsResult = await cliente.GetNewsAsync(page);
            var result = newsResult switch
            {
                DialengaErrorDTO error => new OkObjectResult(error),
                _ => new OkObjectResult(newsResult)
            };

            return result;

        }
        catch (HttpRequestException ex)
        {
            Log.Logger.ForContext("Process", "Inbox").Error(ex, "HttpRequestException thrown getting data from Vivec");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        catch (InvalidOperationException ex)
        {
            Log.Logger.ForContext("Process", "Inbox").Error(ex, ex.Message);
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

namespace Vrt.Vivec.Svc.Application;

public interface INewsAppService
{
    Task<IActionResult> NewsAsync(string  token);
}

public class NewsAppService : INewsAppService
{
    public async Task<IActionResult> NewsAsync(string token)
    {
        if(token == null || string.IsNullOrEmpty(token))
        {
            throw new InvalidOperationException("Token is null or empty.");
        }

        return null;
    }
}

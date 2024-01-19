namespace Vrt.Vivec.Svc.Application;

public interface INewsAppService
{
    Task<IActionResult> NewsAsync(string  token);
}

public class NewsAppService : INewsAppService
{
    public Task<IActionResult> NewsAsync(string token)
    {
        throw new NotImplementedException();
    }
}

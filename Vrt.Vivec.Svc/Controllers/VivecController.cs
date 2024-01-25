

namespace Vrt.Vivec.Svc.Controllers;


[Route("api/v1/[controller]")]
[ApiController]
public class VivecController : ControllerBase
{
    private readonly INewsAppService _newsAppService;

    public VivecController(IConfiguration configuration, INewsAppService newsAppService)
    {
        _newsAppService = newsAppService ?? throw new ArgumentNullException(nameof(newsAppService));
    }

    [HttpGet("News")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(DialengaErrorDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(DialengaErrorDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NewsHtmlDTO), StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)] 
    public async Task<IActionResult> GetNews([FromQuery] int page)
    {

        try
        {
            var result = await _newsAppService.NewsAsync(page);
            if (result == null) return BadRequest("The request is not valid.");

            return result;
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex?.Message);
        }
    }

}

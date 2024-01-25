

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
    [ProducesResponseType(typeof(NewsHtmlDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetNews([FromQuery] int page)
    {

        try
        {
            var result = await _newsAppService.NewsAsync(page);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex?.Message);
        }
    }

}

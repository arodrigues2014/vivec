

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

    [HttpGet("News/page")]
    [ProducesResponseType(typeof(NewsHtmlDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(DialengaErrorDTO), StatusCodes.Status401Unauthorized)]

    public Task<IActionResult> News([FromQuery] int page) => _newsAppService.NewsAsync(page);

}

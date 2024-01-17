
namespace Vrt.Vivec.Svc.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class VivecController : ControllerBase
    {
        private readonly IUsuarioValidator _usuarioValidator;

        public VivecController(IUsuarioValidator usuarioValidator)
        {
            _usuarioValidator = usuarioValidator ?? throw new ArgumentNullException(nameof(usuarioValidator));
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromQuery] Usuario usuario)
        {

            try
            {
                var validationResult = _usuarioValidator.Validate(usuario);

                if (validationResult.IsValid)
                {
                    return Ok(true);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResultHelper(usuario, validationResult.Errors));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}

using Vrt.Vivec.Svc.Data;

namespace Vrt.Vivec.Svc.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class VivecController : ControllerBase
    {
        [HttpGet("GetPin/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(false);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromQuery] UsuarioDTO usuarioDTO)
        {
            try
            {

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(false);
            }
        }
    }
}

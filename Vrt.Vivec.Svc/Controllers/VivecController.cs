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
                // Manejar cualquier excepción y devolver false o algún indicador de error
                return BadRequest(false);
            }
        }

    }
}

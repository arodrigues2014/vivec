
namespace Vrt.Vivec.Svc.Controllers;


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
    //[Authorize]
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

    [HttpGet]
    //[Authorize]
    [ProducesResponseType(typeof(News), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
    public IActionResult GetNews()
    {
        // Construir el objeto NewsResponse con datos simulados
        var newsResponse = new NewsResponse
        {
            Page = 0,
            PageElements = 10,
            TotalPages = 4,
            TotalElements = 37,
            LastPage = false,
            TotalUnread = 34,
            Messages = new List<Message>
        {
            new Message
            {
                Id = 40643,
                Category = new Category
                {
                    Id = 2784,
                    Name = "Tecnología",
                    Color = 4936020,
                    ParentId = null,
                    Language = "es",
                    ImageURL = "https://platform.dialenga.com/api/resources/acd4281e-0e7d-4cac-8693-986a9d520d24"
                },
                PublicationDate = 1700071233000,
                State = 3,
                ScheduleKind = 2,
                Population = 3,
                LastModifiedDate = 1700072393000,
                Language = "es",
                AvailableLanguages = new List<string> { "es" },
                Title = "\uD83D\uDC49Cambios en APP Cabify: Podrás escoger el viaje que te convenga",
                Text = "<p>La aplicaci&oacute;n de Cabify ha implementado <strong>tres mejoras</strong> que har&aacute;n tu d&iacute;a a d&iacute;a m&aacute;s sencillo. Estos son los cambios que podr&aacute;s visualizar:</p><p><strong>1. Una nueva barra inferior&nbsp;</strong><br />Podr&aacute;s escoger el viaje que m&aacute;s te convenga a trav&eacute;s de una navegaci&oacute;n m&aacute;s f&aacute;cil. Tendr&aacute;s acceso directo a 4 secciones:&nbsp;</p><ul><li>Viajes&nbsp;</li><li>Mapa&nbsp;</li><li>Finanzas&nbsp;</li><li>Reservas<strong>*&nbsp;</strong><br /><strong>*</strong><em>Esta opci&oacute;n solo estar&aacute; disponible para los conductores con una tasa de finalizaci&oacute;n mayor al 95%.&nbsp;</em></li></ul><p><img csp-src=\"https://platform.dialenga.com/api/resources/ba6b5e0e-f638-4b13-969c-d0f7075ce9a2\" src=\"https://platform.dialenga.com/api/resources/ba6b5e0e-f638-4b13-969c-d0f7075ce9a2\" /></p><p><strong>&nbsp;</strong></p><h2><strong>2. Panel de viajes &nbsp;&nbsp;</strong></h2><h2><strong></strong>En lugar de ver los viajes disponibles en el mapa, podr&aacute;s verlos en el panel de viajes al iniciar sesi&oacute;n y ponerte en marcha de manera m&aacute;s r&aacute;pida.&nbsp;<br /><img csp-src=\"https://platform.dialenga.com/api/resources/8b46567e-f6ca-46e1-b8a1-f874668619e1\" src=\"https://platform.dialenga.com/api/resources/8b46567e-f6ca-46e1-b8a1-f874668619e1\" /><br /><br /><strong>3. Bot&oacute;n superior para entrar y salir de servicio. &nbsp;</strong></h2><p><strong></strong><br />Estar&aacute; en la zona superior de la pantalla a la hora de iniciar el servicio. Tambi&eacute;n podr&aacute;s encontrarlo dentro del men&uacute; desplegable para salir de servicio. &nbsp;&nbsp;<br /><br /><img csp-src=\"https://platform.dialenga.com/api/resources/5c714781-6527-49aa-bd6f-5610464cfca8\" src=\"https://platform.dialenga.com/api/resources/5c714781-6527-49aa-bd6f-5610464cfca8\" /><br /></p><p>&nbsp;</p><p>Una experiencia m&aacute;s sencilla para mejores decisiones<br /></p>&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",
                ImageURL = "https://platform.dialenga.com/api/resources/616ee39a-72c0-4a8d-b17a-74bae66699eb",
                Read = false,
                Answered = false,
                Kind = 1,
                Highlighted = false,
                Shareable = false,
                DialengaHappinessScore = 3.4583,
                TotalVotes = 48,
                CommentsEnabled = false,
                TotalComments = 0,
                AllowImageZooming = false
            }
        }
        };

        var settings = new JsonSerializerSettings
        {
            Converters = { new EpochDateTimeConverterHelper() }
        };

        // Serializar el objeto a JSON
        var jsonResult = JsonConvert.SerializeObject(newsResponse);

        NewsDTO newsDTO = JsonConvert.DeserializeObject<NewsDTO>(jsonResult, settings);

        // Crear una instancia del mapeador
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

        News news = mapper.Map<NewsDTO, News>(newsDTO);

        // Devolver el JSON como resultado de la solicitud
        return Ok(news);
    }
}

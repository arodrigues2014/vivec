namespace Vrt.Vivec.Svc;

public partial class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => this.Configuration = configuration;
}

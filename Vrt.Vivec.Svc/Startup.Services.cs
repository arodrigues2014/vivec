

namespace Vrt.Vivec.Svc;

public partial class Startup
{
    public void AddDomainConfiguration(IServiceCollection services)
    {
        this.AddServices(services);
    }

    private void AddServices(IServiceCollection services)
    {
        services.AddTransient<INewsAppService, NewsAppService>();
        services.AddAutoMapper(typeof(MappingProfile));
    }
}

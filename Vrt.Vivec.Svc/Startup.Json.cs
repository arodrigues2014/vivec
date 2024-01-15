using Newtonsoft.Json.Serialization;

namespace Vrt.Vivec.Svc;

public partial class Startup
{
    public void AddJsonSettings(IServiceCollection services)
    {
        services
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
    }
}

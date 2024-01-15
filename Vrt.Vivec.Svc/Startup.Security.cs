using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;

namespace Vrt.Vivec.Svc;

public partial class Startup
{
    public void AddAuthentication(IServiceCollection services, bool isProduction)
    {
        AuthenticationBuilder authBuilder = services.AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme);

        authBuilder.AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = isProduction;
            options.Authority = this.Configuration.GetValue<string>("Authentication:Authority");
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidAudiences = this.Configuration.GetSection("Authentication:Audiences").Get<string[]>()
            };
        });

        services.AddAuthorization();
    }
}
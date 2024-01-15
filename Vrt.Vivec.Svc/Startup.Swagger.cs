using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Vrt.Vivec.Svc.Filters;
using Vrt.Vivec.Svc.Helpers;

namespace Vrt.Vivec.Svc;

public partial class Startup
{
    public void AddSwaggerGen(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            IConfigurationSection? section = this.Configuration.GetSection("Swagger:Authentication:Scopes");

            Dictionary<string, string> scopes = new Dictionary<string, string>(section
                .GetChildren()
                .Select(scope =>
                    new KeyValuePair<string, string>(
                        scope.GetValue<string>("Scope")!,
                        scope.GetValue<string>("Description")!)));

            OpenApiSecurityScheme scheme = new OpenApiSecurityScheme
            {
                Description = "OAuth2 Authentication",
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{this.Configuration.GetValue<string>("Swagger:Authentication:Authority")}{this.Configuration.GetValue<string>("Swagger:Authentication:AuthorizationEndPoint")}"),
                        TokenUrl = new Uri($"{this.Configuration.GetValue<string>("Swagger:Authentication:Authority")}{this.Configuration.GetValue<string>("Swagger:Authentication:TokenEndPoint")}"),
                        Scopes = scopes
                    }
                }
            };

            options.AddSecurityDefinition("oauth2", scheme);

            options.OperationFilter<DefaultValuesOperationFilter>();
            options.OperationFilter<AuthorizeCheckOperationFilter>();
        });

        services.AddSwaggerGenNewtonsoftSupport();
    }

    public void ConfigureSwagger(WebApplication app)
    {
        IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwagger(options =>
        {
            options.SerializeAsV2 = true;

            foreach (ApiVersionDescription apiDescription in provider.ApiVersionDescriptions)
            {
                options.PreSerializeFilters.Add((document, request) =>
                {
                    if (request.Headers.ContainsKey("X-Forwarded-Host"))
                    {
                        string basePath = $"{(!app.Environment.IsDevelopment() ? $"vivec" : "")}";
                        string serverUrl = $"https://{request.Headers["X-Forwarded-Host"]}/{basePath}";
                        document.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
                    }

                    document.Info = CreateApiVersionInfo(apiDescription);
                });
            };
        });

        app.UseSwaggerUI(options =>
        {
            foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
            {
                options.OAuthClientId(SettingsHelper.SwaggerAuthenticationCliendId);

                options.OAuthAppName(SettingsHelper.SwaggerAuthenticationAppName);

                options.SwaggerEndpoint(
                    $"{(!app.Environment.IsDevelopment() ? $"/vivec" : "")}/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });
    }

    private OpenApiInfo CreateApiVersionInfo(ApiVersionDescription apiDescription) =>
        new OpenApiInfo
        {
            Title = $"{SettingsHelper.SwaggerInfoApiTitle} {apiDescription.ApiVersion}",
            Version = apiDescription.ApiVersion.ToString(),
            Description = !apiDescription.IsDeprecated ?
                SettingsHelper.SwaggerInfoApiDescription :
                SettingsHelper.SwaggerInfoApiDeprecated,
            Contact = new OpenApiContact
            {
                Email = SettingsHelper.SwaggerInfoApiContactEmail,
                Name = SettingsHelper.SwaggerInfoApiContactName,
                Url = new Uri(SettingsHelper.SwaggerInfoApiContactUrl ?? string.Empty)
            }
        };
}

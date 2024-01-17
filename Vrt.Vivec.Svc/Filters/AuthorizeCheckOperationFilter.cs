namespace Vrt.Vivec.Svc.Filters;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        bool hasAuthorize = context
            .MethodInfo
            .CustomAttributes
            .Any(p => p.AttributeType == typeof(AuthorizeAttribute));
        
        if (!hasAuthorize) return;

        operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
        operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

        OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "oauth2"
            },
            Type = SecuritySchemeType.OAuth2,
            Name = "oauth2",
        };

        OpenApiSecurityRequirement requirement = new OpenApiSecurityRequirement();

        requirement.Add(securityScheme, new List<string>());

        operation.Security.Add(requirement);
    }
}

﻿
namespace Vrt.Vivec.Svc.Filters;

public class DefaultValuesOperationFilter : IOperationFilter
{
    /// <summary>
    /// Applies the filter to the specified operation using the given context.
    /// </summary>
    /// <param name="operation">The operation to apply the filter to.</param>
    /// <param name="context">The current operation filter context.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters is null) return;

        var parametersToRemove = operation.Parameters.Where(x => x.Name == "api-version").ToList();
        foreach (var parameter in parametersToRemove)
            operation.Parameters.Remove(parameter);

        foreach (OpenApiParameter parameter in operation.Parameters)
        {
            ApiParameterDescription? description = context
                .ApiDescription
                .ParameterDescriptions
                .FirstOrDefault(p => p.Name == parameter.Name);

            if (description is not null)
            {
                if (string.IsNullOrEmpty(parameter.Description)) parameter.Description = description.ModelMetadata.Description;

                if (description.RouteInfo is null || description.RouteInfo.DefaultValue is null) continue;

                if (parameter.Schema.Default is null) parameter.Schema.Default = new OpenApiString(description.RouteInfo.DefaultValue.ToString());

                parameter.Required |= !description.RouteInfo.IsOptional;
            }
        }
    }
}


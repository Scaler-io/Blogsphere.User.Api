﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Blogsphere.Swagger;
public sealed class SwaggerHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var headers = context.MethodInfo?.GetCustomAttributes(true).OfType<SwaggerHeaderAttribute>();
        operation.Parameters ??= [];


        foreach(var header in headers)
        {
            operation.Parameters.Add(new()
            {
                Name = header.Name,
                In = ParameterLocation.Header,
                Description = header.Description,
                Schema = new() { Type = header.Type ?? "string" },
                Required = header.Required
            });
        }
    }
}

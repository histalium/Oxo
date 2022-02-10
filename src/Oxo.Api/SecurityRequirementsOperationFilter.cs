using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Oxo.Api;

internal class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var anonControllerScope = context?
            .MethodInfo?
            .DeclaringType?
            .GetCustomAttributes(true)?
            .OfType<AllowAnonymousAttribute>() ?? Enumerable.Empty<AllowAnonymousAttribute>();

        var anonMethodScope = context?
            .MethodInfo?
            .GetCustomAttributes(true)?
            .OfType<AllowAnonymousAttribute>() ?? Enumerable.Empty<AllowAnonymousAttribute>();

        if (anonMethodScope.Any() || anonControllerScope.Any())
        {
            return;
        }

        operation.Responses
            .AddIfNotContains("401", "If Authorization header not present, has no value or no valid jwt bearer token")
            .AddIfNotContains("403", "If user not authorized to perform requested action");

        var jwtAuthScheme = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        };

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                { jwtAuthScheme, new List<string>() }
            }
        };
    }
}

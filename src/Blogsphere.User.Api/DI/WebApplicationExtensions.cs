using Asp.Versioning.ApiExplorer;
using Blogsphere.Swagger;
using Blogsphere.User.Api.Middlewares;
using HealthChecks.UI.Client;
using Scalar.AspNetCore;

namespace Blogsphere.User.Api.DI;

public static class WebApplicationExtensions
{
    public static WebApplication AddApplicationPipeline(this WebApplication app, SwaggerConfiguration swaggerConfiguration)
    {
        app.UseSwagger(SwaggerConfiguration.SetupSwaggerOptions);
        app.UseSwaggerUI(options =>
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            SwaggerConfiguration.SetupSwaggerUiOptions(options, provider);
            foreach(var description in provider.ApiVersionDescriptions)
            {
                app.MapScalarApiReference($"scalar/{description.GroupName}", options => 
                {
                    SwaggerConfiguration.SetupScalarOptions(options, description);
                });
            }
        });

        app.MapHealthChecks("/healthcheck", new()
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });

        app.MapHealthChecksUI(options => options.UIPath = "/dashboard");

        
        app.UseMiddleware<CorrelationHeaderEnricher>()
            .UseMiddleware<RequestLoggerMiddleware>()
            .UseMiddleware<GlobalExceptionMiddleware>();
        
        app.UseCors("blogspherecors");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}

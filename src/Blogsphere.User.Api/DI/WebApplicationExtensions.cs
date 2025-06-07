using Asp.Versioning.ApiExplorer;
using Blogsphere.Swagger;
using Blogsphere.User.Api.Middlewares;
using HealthChecks.UI.Client;

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
        });

        app.UseHttpsRedirection();

        app.UseMiddleware<CorrelationHeaderEnricher>()
            .UseMiddleware<RequestLoggerMiddleware>()
            .UseMiddleware<GlobalExceptionMiddleware>();

        app.MapHealthChecks("/healthcheck", new()
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });

        app.MapHealthChecksUI(options => options.UIPath = "/dashboard");


        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.UseCors("ecoedencors");

        return app;
    }
}

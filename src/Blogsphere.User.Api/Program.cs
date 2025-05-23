using Blogsphere.Swagger;
using Blogsphere.User.Api;
using Blogsphere.User.Api.DI;
using Blogsphere.User.Infrastructure.DI;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


var apiName = SwaggerConfiguration.ExtractApiNameFromEnvironmentVariable();
var apiDescription = builder.Configuration["ApiDescription"];
var apiHost = builder.Configuration["ApiOriginHost"];
var swaggerConfiguration = new SwaggerConfiguration(apiName, apiDescription, apiHost, builder.Environment.IsDevelopment());


builder.Services
    .ConfigurationApplicationOptions(builder.Configuration)
    .AddApplicationServices(builder.Configuration, swaggerConfiguration)
    .ConfigureInfraServices(builder.Configuration);

var logger = Logging.GetLogger(builder.Configuration, builder.Environment);
builder.Host.UseSerilog(logger);

var app = builder.Build();

app.AddApplicationPipeline(swaggerConfiguration);


try
{
    await app.RunAsync();
}
finally
{
    Log.CloseAndFlush();
}
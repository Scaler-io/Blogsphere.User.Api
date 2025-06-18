using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Blogsphere.Swagger;
using Blogsphere.Swagger.Examples.HealthCheck;
using Blogsphere.Swagger.Examples.UserRegistration;
using Blogsphere.User.Api.Middlewares;
using Blogsphere.User.Api.Services;
using Blogsphere.User.Domain.Configurations;
using Blogsphere.User.Domain.Models.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Swashbuckle.AspNetCore.Filters;

namespace Blogsphere.User.Api.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, SwaggerConfiguration swaggerConfiguration)
    {
        services.AddControllers()
             .AddNewtonsoftJson(config =>
             {
                 config.SerializerSettings.ContractResolver = new DefaultContractResolver
                 {
                     NamingStrategy = new CamelCaseNamingStrategy()
                 };

                 config.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
                 config.SerializerSettings.Converters.Add(new StringEnumConverter());
             });

        // swagger generation set up
        services.AddEndpointsApiExplorer();

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = ApiVersion.Default;
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddHealthChecks();

        services.AddHealthChecksUI(options =>
        {
            options.AddHealthCheckEndpoint("Blogspher Api Health", "/healthcheck");
        }).AddInMemoryStorage();

        // handles api's default error validation model
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = HandleFrameworkValidationFailure();
        });

        // swagger setup
        services.AddSwaggerExamplesFromAssemblies(typeof(HealthCheckResponseExample).Assembly);
        services.AddSwaggerExamples();
        services.AddSwaggerGen(options =>
        {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            swaggerConfiguration.SetupSwaggerGenService(options, provider);
        });

        services.AddTransient<CorrelationHeaderEnricher>()
            .AddTransient<GlobalExceptionMiddleware>()
            .AddTransient<RequestLoggerMiddleware>();

        // configure identity
        var identityGroupAccess = configuration.GetSection(IdentityGroupAccessOption.OptionName)
            .Get<IdentityGroupAccessOption>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.Audience = identityGroupAccess.Audience;
                options.Authority = identityGroupAccess.Authority;
                options.TokenValidationParameters = new()
                {
                    ValidateAudience = true,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireClaim("scope", "userapi:read")
                .RequireClaim("scope", "userapi:write")
                .RequireAuthenticatedUser()
                .Build();
        });

        services.AddHttpContextAccessor();
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("blogsphere.user.api"))
            .WithTracing(tracing =>
            {
                tracing.AddSource("Blogsphere.User.API")
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddSqlClientInstrumentation(options => options.SetDbStatementForText = true)
                .AddRedisInstrumentation()
                .AddZipkinExporter(options =>
                {
                    options.Endpoint = new Uri(configuration["Zipkin:Url"]);
                })
                .AddMassTransitInstrumentation();
            });

        services.AddCors(options =>
        {
            options.AddPolicy("ecoedencors", policy =>
            {
                policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            });
        });

        return services;
    }

    private static Func<ActionContext, IActionResult> HandleFrameworkValidationFailure()
    {
        return context =>
        {
            var errors = context.ModelState
            .Where(m => m.Value.Errors.Count > 0)
            .ToList();

            var validationError = new ApiValidationResponse
            {
                Errors = []
            };

            foreach (var error in errors)
            {
                foreach (var subError in error.Value.Errors)
                {
                    validationError.Errors.Add(new FieldLevelError
                    {
                        Field = error.Key,
                        Message = subError.ErrorMessage
                    });
                }
            }

            return new BadRequestObjectResult(validationError);
        };
    }
}

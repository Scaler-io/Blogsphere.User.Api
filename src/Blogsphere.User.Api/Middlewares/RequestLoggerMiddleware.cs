
using Blogsphere.User.Application.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog.Context;
using System.Diagnostics;

namespace Blogsphere.User.Api.Middlewares;

public class RequestLoggerMiddleware(ILogger logger) : IMiddleware
{
    private readonly ILogger _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();

        var headers = context.Request.Headers.ToDictionary(x => x.Key, x => x.Value);
        var url = context.Request.GetDisplayUrl();
        var verb = context.Request.Method;


        using(LogContext.PushProperty("Url", url))
        {
            LogContext.PushProperty("HttepMethod", verb);
            _logger.Here().Information("Http request starting...");

            await next(context);

            stopWatch.Stop();
            _logger.Here().Debug("Elapsed time {elapsedTime}", stopWatch.Elapsed);
            _logger.Here().Information("Http request completed. Response code {@code}", context.Response.StatusCode);
        }

    }
}

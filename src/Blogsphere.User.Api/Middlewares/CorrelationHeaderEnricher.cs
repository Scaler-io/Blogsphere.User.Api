﻿
using Blogsphere.User.Api.Extensions;
using Blogsphere.User.Domain.Models.Constants;
using Serilog.Context;

namespace Blogsphere.User.Api.Middlewares;

public sealed class CorrelationHeaderEnricher : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var correlationId = context?.Request.GetRequestHeaderOrDefault(LoggerConstants.CorrelationId, $"GEN-{Guid.NewGuid()}");
        using (LogContext.PushProperty("ThreadId", Environment.CurrentManagedThreadId))
        {
            LogContext.PushProperty(LoggerConstants.CorrelationId, correlationId);
            context.Request.Headers.Append(LoggerConstants.CorrelationId, correlationId);
            await next(context);
        }
    }
}

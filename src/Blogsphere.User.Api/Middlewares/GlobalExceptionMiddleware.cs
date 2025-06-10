
using Blogsphere.User.Domain.Models.Core;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;
using Newtonsoft.Json.Converters;
using Blogsphere.User.Domain.Models.Enums;
using Blogsphere.User.Application.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blogsphere.User.Api.Middlewares;

public class GlobalExceptionMiddleware(ILogger logger, IWebHostEnvironment environment) : IMiddleware
{
    private readonly ILogger _logger = logger;
    private readonly IWebHostEnvironment _environment = environment;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleGlobalException(context, ex);
        }
    }

    private async Task HandleGlobalException(HttpContext context, Exception ex)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        var jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters =
            [
                new StringEnumConverter()
            ]
        };

        if(ex is ValidationException validationException)
        {
            await HandleValidationException(context, validationException, jsonSettings);
        }
        else
        {
            await HandleGeneralException(context, ex, jsonSettings);
        }
    }

    private async Task HandleGeneralException(HttpContext context, Exception ex, JsonSerializerSettings settings)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var response = _environment.IsDevelopment()
            ? new ApiExceptionResponse(ex.Message, ex.StackTrace)
            : new ApiExceptionResponse(ex.Message);

        var jsonResponse = JsonConvert.SerializeObject(response, settings);

        _logger.Here().Error("{@InternalServerError} - {BeautifiedJson}", ErrorCodes.InternalServerError, jsonResponse);

        await context.Response.WriteAsync(jsonResponse);
    }

    private async Task HandleValidationException(HttpContext context, ValidationException validationException, JsonSerializerSettings settings)
    {

        #region obsolte
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        var apiValidationResponse = new ApiValidationResponse
        {
            Errors = []
        };

        var fieldLevelError = validationException.Errors.Select(e => new FieldLevelError { Code = e.ErrorCode, Field = e.PropertyName, Message = e.ErrorMessage }).ToList();
        apiValidationResponse.Errors.AddRange(fieldLevelError);

        _logger.Here().Error("{@BadRequest} - {@response}", ErrorCodes.BadRequest, apiValidationResponse);
        await context.Response.WriteAsync(JsonConvert.SerializeObject(apiValidationResponse, settings));
        #endregion

        // Ensure ModelState is initialized in HttpContext.Items
        context.Items["ModelState"] ??= new ModelStateDictionary();
        var modelState = (ModelStateDictionary)context.Items["ModelState"];

        // Add FluentValidation errors to ModelState
        foreach (var error in validationException.Errors)
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        // Trigger the InvalidModelStateResponseFactory by setting ModelState and ending the pipeline
        await context.Response.CompleteAsync();  // Stops the request pipeline here
    }
}

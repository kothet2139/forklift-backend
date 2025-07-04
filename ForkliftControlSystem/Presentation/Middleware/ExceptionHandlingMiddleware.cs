using ForkliftControlSystem.Application.DTOs;
using ForkliftControlSystem.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace ForkliftControlSystem.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // continue pipeline
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        int statusCode = ex switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;

        var response = new ApiResponse<object>
        {
            Success = false,
            Data = null,
            Error = new ApiError { Message = ex.Message }
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

}

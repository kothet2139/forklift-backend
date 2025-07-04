public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        // Log request
        context.Request.EnableBuffering();
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;

        _logger.LogInformation("HTTP Request: {method} {path} | Body: {body}",
            context.Request.Method,
            context.Request.Path,
            requestBody);

        // Capture response
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context); // Continue pipeline

        responseBody.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(responseBody).ReadToEndAsync();
        responseBody.Seek(0, SeekOrigin.Begin);

        _logger.LogInformation("HTTP Response: {statusCode} | Body: {response}",
            context.Response.StatusCode,
            responseText);

        await responseBody.CopyToAsync(originalBodyStream);
    }
}

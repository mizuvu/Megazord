using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Zord.Api.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next,
    ILogger<RequestLoggingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger _logger = logger; // must use Microsoft Logger because only Singleton services can be resolved by constructor injection in Middleware
    private readonly string[] _excludePath = ["hangfire", "swagger"];

    public async Task InvokeAsync(HttpContext context)
    {
        HttpRequest request = context.Request;

        var traceId = context.TraceIdentifier;
        var ip = request.HttpContext.Connection.RemoteIpAddress;
        var clientIp = ip == null ? "UnknownIP" : ip.ToString();

        var requestMethod = request.Method;
        var requestPath = request.Path;
        var requestQuery = request.QueryString.ToString();
        //var requestScheme = request.Scheme;
        //var requestHost = request.Host.ToString();
        //var requestContentType = request.ContentType;

        var requestBody = await ReadBodyAsync(request);

        bool notWriteFrom = _excludePath.Any(c => requestPath.ToString().Contains(c));

        if (notWriteFrom)
        {
            await _next(context);
        }
        else
        {
            // Create a new memory stream to capture the response
            var originalBody = context.Response.Body;

            try
            {
                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                // Continue processing the request
                await _next(context);

                // Read the response body
                responseBody.Seek(0, SeekOrigin.Begin);
                string responseText = new StreamReader(responseBody).ReadToEnd();

                // Log the response body
                var resource = $"[{request.Method}] {requestPath}{requestQuery}";
                var statusCode = context.Response.StatusCode;
                var contentType = context.Response.Headers.ContentType.ToString();
                var logContent = $"{Minify(requestBody)}\r\nStatus Code: {statusCode}\r\nTrace ID: {traceId}\r\n{contentType}\r\n{responseText}";
                MiddlewareLogger.Write(clientIp, resource, logContent);

                // Copy the response body back to the original stream
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBody);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandler exception with error {error}.", ex.Message);
            }
            /*
            finally
            {
                context.Response.Body = originalBody;
            }
            */
        }
    }

    private static async Task<string> ReadBodyAsync(HttpRequest request)
    {
        // Ensure the request's body can be read multiple times 
        // (for the next middlewares in the pipeline).
        request.EnableBuffering();
        using var streamReader = new StreamReader(request.Body, leaveOpen: true);
        var requestBody = await streamReader.ReadToEndAsync();
        // Reset the request's body stream position for 
        // next middleware in the pipeline.
        request.Body.Position = 0;
        return requestBody;
    }

    private static string Minify(string json)
    {
        if (string.IsNullOrEmpty(json))
            return "<null>";

        var obj = JsonSerializer.Deserialize<object>(json);
        return JsonSerializer.Serialize(obj);
    }
}

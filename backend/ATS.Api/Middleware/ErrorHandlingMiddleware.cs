using ATS.Api.Exceptions;
using System.Text.Json;

namespace ATS.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new { error = ex.Message });
            await context.Response.WriteAsync(result);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new { error = "Något gick fel", detail = ex.Message });
            await context.Response.WriteAsync(result);
        }
    }
}
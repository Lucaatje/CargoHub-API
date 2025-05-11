using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, DatabaseContext dbContext)
    {
        if (!context.Request.Headers.TryGetValue("API_KEY", out var extractedApiKey))
        {
            context.Response.StatusCode = 401; 
            await context.Response.WriteAsync("API Key is missing");
            await AuditLogService.LogAPIKeyAsync(context.Request.Method, "401 UNAUTHORIZED: Missing API Key", extractedApiKey);
            return;
        }

        var apiKeys = await dbContext.Api_Keys.ToListAsync();
        var apiKeyEntity = apiKeys.FirstOrDefault(key => BCrypt.Net.BCrypt.Verify(extractedApiKey, key.ApiKey));

        if (apiKeyEntity == null)
        {
            context.Response.StatusCode = 403; 
            await context.Response.WriteAsync("Invalid API Key");
            await AuditLogService.LogAPIKeyAsync(context.Request.Method, "403 FORBIDDEN: Invalid API Key", extractedApiKey);
            return;
        }

        await AuditLogService.LogAPIKeyAsync(context.Request.Method, "200 OK: Valid API Key used", extractedApiKey);
        await _next(context);
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace NEvo.Web.Middlewares;

public class DebugConfigurationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public DebugConfigurationMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<DebugConfigurationMiddleware> logger)
    {
        _next = next;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/debug/configuration")
        {
            var configurationObject = CreateConfigurationObject(_configuration);

            var configurationJson = JsonSerializer.Serialize(configurationObject, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(configurationJson);
        }
        else
        {
            await _next(context);
        }
    }

    private static IDictionary<string, object?> CreateConfigurationObject(IConfiguration configuration) 
        => configuration.GetChildren().ToDictionary(
            section => section.Key,
            section =>
            {
                var children = section.GetChildren();
                return children.Any() ? CreateConfigurationObject(section) : (object?)section.Value;
            });
}

public static class ConfigurationMiddlewareExtensions
{
    public static IApplicationBuilder UseDebugConfigurationMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<DebugConfigurationMiddleware>();
        return app;
    }
}
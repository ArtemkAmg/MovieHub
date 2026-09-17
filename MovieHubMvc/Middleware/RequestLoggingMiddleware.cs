using System.Text;

namespace MovieHubMvc.Middleware;

/// <summary>
/// Logs every HTTP request: full URL (with query string), timestamp, client IP.
/// Writes to Logs/requests-yyyy-MM-dd.log in the content root.
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private static readonly object _lock = new();

    public RequestLoggingMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;

        // Full URL including scheme, host, path and query string
        // Query string is the part after '?' e.g. /Movies?genre=Action&page=2
        var fullUrl = $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString}";

        var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        // X-Forwarded-For if behind proxy
        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwarded))
        {
            ip = forwarded.ToString().Split(',')[0].Trim();
        }

        var line = $"[{time}] URL: {fullUrl} | IP: {ip}{Environment.NewLine}";

        var logsDir = Path.Combine(_env.ContentRootPath, "Logs");
        Directory.CreateDirectory(logsDir);

        var filePath = Path.Combine(logsDir, $"requests-{DateTime.Now:yyyy-MM-dd}.log");

        lock (_lock)
        {
            File.AppendAllText(filePath, line, Encoding.UTF8);
        }

        await _next(context);
    }
}

public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestLoggingMiddleware>();
    }
}

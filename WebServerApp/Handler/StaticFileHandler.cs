using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;  // Add this using for ILogger
using WebServerApp.Http;

namespace WebServerApp.Handler;

public class StaticFileHandler : IHandler
{
    private readonly string _root;
    private readonly ILogger<StaticFileHandler> _logger;

    public StaticFileHandler(string root, ILogger<StaticFileHandler> logger)
    {
        _root = root;
        _logger = logger;
    }

    public async Task Handle(HttpContext context)
    {
        var requestPath = context.Request.Url.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
        var path = Path.GetFullPath(Path.Combine(_root, requestPath));

        _logger.LogInformation("Handling request for {RequestUrl} resolved to {ResolvedPath}", context.Request.Url, path);

        if (!path.StartsWith(Path.GetFullPath(_root))) // Prevent directory traversal
        {
            _logger.LogWarning("Directory traversal attempt detected: {ResolvedPath}", path);
            context.Response.StatusCode = "403 Forbidden";
            context.Response.ContentType = "text/html";
            context.Response.Content = LoadErrorPage();
            return;
        }

        if (File.Exists(path))
        {
            var contentType = GetContentType(path);
            if (contentType == null)
            {
                _logger.LogWarning("Unsupported file type requested: {FilePath}", path);
                context.Response.StatusCode = "403 Forbidden";
                context.Response.ContentType = "text/html";
                context.Response.Content = LoadErrorPage();
                return;
            }
            context.Response.Content = await File.ReadAllBytesAsync(path);
            context.Response.ContentType = contentType;

            _logger.LogInformation("Served file {FilePath} with content type {ContentType}", path, contentType);
        }
        else
        {
            _logger.LogWarning("File not found: {FilePath}", path);
            context.Response.StatusCode = "404 Not Found";
            context.Response.ContentType = "text/html";
            context.Response.Content = LoadErrorPage();
        }
    }

    private string? GetContentType(string path)
    {
        return Path.GetExtension(path).ToLowerInvariant() switch
        {
            ".html" => "text/html",
            ".css" => "text/css",
            ".js" => "application/javascript",
            ".json" => "application/json",
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            _ => null
        };
    }

    private byte[] LoadErrorPage()
    {
        string errorPagePath = Path.Combine(_root, "error.html");
        if (File.Exists(errorPagePath))
        {
            _logger.LogInformation("Serving custom error page from {ErrorPagePath}", errorPagePath);
            return File.ReadAllBytes(errorPagePath);
        }

        _logger.LogWarning("Custom error page not found, serving fallback error message");
        // Fallback
        return Encoding.UTF8.GetBytes("<h1>Error</h1>");
    }
}

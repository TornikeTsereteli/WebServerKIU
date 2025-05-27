
using System.Text;
using WebServerApp.Http;

namespace WebServerApp.Handler;

public class StaticFileHandler : IHandler
{
    private readonly string _root;

    public StaticFileHandler(string root)
    {
        _root = root;
    }

    public void Handle(HttpContext context)
    {
        var requestPath = context.Request.Url.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
        var path = Path.GetFullPath(Path.Combine(_root, requestPath));
        
        if (!path.StartsWith(Path.GetFullPath(_root))) // Prevent directory traversal
        {
            context.Response.StatusCode = "403 Forbidden";
            context.Response.Content = Encoding.UTF8.GetBytes("<h1>403 Forbidden</h1>");
            return;
        }
       
        
        if (!path.StartsWith(Path.GetFullPath(_root))) // Prevent directory traversal
        {
            context.Response.StatusCode = "403 Forbidden";
            context.Response.Content = Encoding.UTF8.GetBytes("<h1>403 Forbidden</h1>");
            return;
        }
        
        if (File.Exists(path))
        {
            context.Response.Content = File.ReadAllBytes(path);
            context.Response.ContentType = GetContentType(path);
        }
        else
        {
            context.Response.StatusCode = "404 Not Found";
            context.Response.Content = Encoding.UTF8.GetBytes("<h1>404 Not Found</h1>");
        }
    }

    private string GetContentType(string path)
    {
        return Path.GetExtension(path).ToLowerInvariant() switch
        {
            ".html" => "text/html",
            ".css" => "text/css",
            ".js" => "application/javascript",
            ".json" => "application/json",
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            _ => "application/octet-stream"
        };
    }
}

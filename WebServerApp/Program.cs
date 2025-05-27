using WebServerApp.Handler;
using WebServerApp.Server;

namespace WebServerApp;

public class Program
{
    public static void Main(string[] args)
    {
        string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "webroot"));
        WebServer server = new WebServer("127.0.0.1", 13000, new StaticFileHandler(projectRoot));
        server.Start();
        
    }
}
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Extensions.Logging;
using WebServerApp.Handler;
using WebServerApp.Infrastructure.DependencyInjection;
using WebServerApp.Server;

namespace WebServerApp;

public class Program
{
    

    public static void Main(string[] args)
    {


        var services = new ServiceCollection();
        services.AddApplicationServices();
        services.AddLoggingWithNLog();
        
        using var provider = services.BuildServiceProvider();

        var server = provider.GetRequiredService<WebServer>();
        server.Start();
        
        
        // string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "webroot"));
        // WebServer server = new WebServer("127.0.0.1", 13000, new StaticFileHandler(projectRoot),logger);
        // server.Start();
        //
    }
}
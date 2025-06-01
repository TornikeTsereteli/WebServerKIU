using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using WebServerApp.Handler;
using WebServerApp.Server;

namespace WebServerApp.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "webroot"));
     
        services.AddSingleton<IHandler>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<StaticFileHandler>>();
            return new StaticFileHandler(projectRoot, logger);
        });


        
        services.AddSingleton<WebServer>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<WebServer>>();
            var handler = provider.GetRequiredService<IHandler>();
            return new WebServer("127.0.0.1", 13000, handler, logger);
        });
        
        return services;
    }
    
    public static IServiceCollection AddLoggingWithNLog(this IServiceCollection services)
    {
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(LogLevel.Trace);
            logging.AddNLog("NLog.config");
        });

        return services;
    }
}
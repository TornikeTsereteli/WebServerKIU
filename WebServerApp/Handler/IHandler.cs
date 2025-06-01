using WebServerApp.Http;

namespace WebServerApp.Handler;

public interface IHandler
{
    Task Handle(HttpContext context);
}
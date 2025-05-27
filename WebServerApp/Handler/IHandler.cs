using WebServerApp.Http;

namespace WebServerApp.Handler;

public interface IHandler
{
    void Handle(HttpContext context);
}
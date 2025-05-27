using WebServerApp.Server;

namespace WebServerApp;

public class Program
{
    public static void Main(string[] args)
    {
        WebServer server = new WebServer("127.0.0.1", 13000);
        server.Start();
    }
}
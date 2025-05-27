using WebServerKIU.Server;

public partial class Program
{
    public static void Main()
    {
        WebServer server = new WebServer("127.0.0.1", 13000);
        server.Start();
    }
}
using System.Net;
using System.Net.Sockets;

namespace WebServerApp.Server;

public class WebServer
{
    private readonly int _port;
    private readonly IPAddress _ipAddress;
    private TcpListener _listener;

    public WebServer(string ipAddress, int port)
    {
        _port = port;
        _ipAddress = IPAddress.Parse(ipAddress);
    }

    public void Start()
    {
        _listener = new TcpListener(_ipAddress, _port);
        _listener.Start();
        Console.WriteLine($"Server started on {_ipAddress}:{_port}");

        while (true)
        {
            TcpClient client = _listener.AcceptTcpClient();
            Console.WriteLine("Client connected.");

            // Use a task instead of a raw thread
            Task.Run(() =>
            {
                ClientHandler handler = new ClientHandler(client);
                handler.HandleClient();
            });
        }
    }

    public void Stop()
    {
        _listener.Stop();
    }
}
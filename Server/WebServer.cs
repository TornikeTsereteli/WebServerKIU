namespace WebServerKIU.Server;

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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
            Console.WriteLine("Waiting for a connection...");
            TcpClient client = _listener.AcceptTcpClient();
            Console.WriteLine("Client connected.");

            // Handle client in a new thread
            Thread thread = new Thread(() =>
            {
                ClientHandler handler = new ClientHandler(client);
                handler.HandleClient();
            });

            thread.Start();
        }
    }

    public void Stop()
    {
        _listener.Stop();
    }
}

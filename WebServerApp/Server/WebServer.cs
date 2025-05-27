using System.Net;
using System.Net.Sockets;
using WebServerApp.Abstract;
using WebServerApp.Handler;
using WebServerApp.Server;

namespace WebServerApp.Server;

public class WebServer
{
    private readonly TcpListener _listener;
    private readonly IHandler _handler;

    public WebServer(string ip, int port, IHandler handler)
    {
        _listener = new TcpListener(IPAddress.Parse(ip), port);
        _handler = handler;
    }

    public void Start()
    {
        _listener.Start();
        Console.WriteLine($"[Server] Started on {_listener.LocalEndpoint}");

        while (true)
        {
            try
            {
                TcpClient client = _listener.AcceptTcpClient();
                Console.WriteLine("[Server] Client connected.");

                Thread thread = new Thread(() =>
                {
                    try
                    {
                        IClientHandler clientHandler = new ClientHandler(client, _handler);
                        clientHandler.Handle();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Client Error] {ex}");
                    }
                    finally
                    {
                        client.Close();
                        Console.WriteLine("[Server] Client connection closed.");
                    }
                });

                thread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Server Error] {ex}");
                // Optionally add some delay or shutdown logic if needed
            }
        }
    }

    public void Stop()
    {
        _listener.Stop();
        Console.WriteLine("[Server] Stopped.");
    }
}


using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using WebServerApp.Abstract;
using WebServerApp.Handler;

namespace WebServerApp.Server;

public class WebServer
{
    private readonly TcpListener _listener;
    private readonly IHandler _handler;
    private readonly ILogger _logger;

    public WebServer(string ip, int port, IHandler handler, ILogger logger)
    {
        _listener = new TcpListener(IPAddress.Parse(ip), port);
        _handler = handler;
        _logger = logger;
    }

    public async Task Start()
    {
        _listener.Start();
        Console.WriteLine($"[Server] Started on {_listener.LocalEndpoint}");

        while (true)
        {
            try
            {
                TcpClient client = _listener.AcceptTcpClient();
               _logger.LogInformation( "[Server] Client connected.");

               async void ThreadStart()
               {
                   try
                   {
                       IClientHandler clientHandler = new ClientHandler(client, _handler);
                       await clientHandler.HandleAsync();
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError($"[Client Error] {ex}");
                   }
                   finally
                   {
                       client.Close();
                       _logger.LogInformation("[Server] Client connection closed.");
                   }
               }

               Thread thread = new Thread(ThreadStart);

                thread.Start();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Server Error] {ex}");
                // Optionally add some delay or shutdown logic if needed
            }
        }
    }

    public void Stop()
    {
        _listener.Stop();
        _logger.LogInformation("[Server] Stopped.");
    }
}


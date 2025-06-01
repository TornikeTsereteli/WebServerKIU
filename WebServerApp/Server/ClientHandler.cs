using System.Net.Sockets;
using System.Text;
using WebServerApp.Abstract;
using WebServerApp.Handler;
using WebServerApp.Http;

namespace WebServerApp.Server;

public class ClientHandler : IClientHandler
{
    private readonly TcpClient _client;
    private readonly IHandler _handler;

    public ClientHandler(TcpClient client, IHandler handler)
    {
        _client = client;
        _handler = handler;
    }

    public async Task HandleAsync()
    {
        using var stream = _client.GetStream();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        using var writer = new BinaryWriter(stream, Encoding.UTF8);

        var requestBuilder = new StringBuilder();
        string? line;
        while (!string.IsNullOrWhiteSpace(line = reader.ReadLine()))
        {
            requestBuilder.AppendLine(line);
        }

        var request = HttpRequest.Parse(requestBuilder.ToString());
        var context = new HttpContext(request);

        if (!HttpValidator.IsValid(request, out var error))
        {
            context.Response.StatusCode = error;
            context.Response.Content = Encoding.UTF8.GetBytes($"<h1>{error}</h1>");
        }
        else
        {
            await _handler.Handle(context);
        }

        // writer.Write(context.Response.ToBytes());
        context.Response.WriteTo(stream);
        _client.Close();
    }
}
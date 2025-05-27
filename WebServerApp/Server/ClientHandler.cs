using System.Net.Sockets;
using System.Text;

namespace WebServerApp.Server;

public class ClientHandler
{
    private readonly TcpClient _client;

    public ClientHandler(TcpClient client)
    {
        _client = client;
    }

    public void HandleClient()
    {
        using NetworkStream stream = _client.GetStream();
        byte[] buffer = new byte[1024];

        try
        {
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            if (bytesRead > 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received: {message}");

                if (int.TryParse(message.Trim(), out int number))
                {
                    int result = number * number;
                    byte[] response = Encoding.UTF8.GetBytes(result.ToString());
                    stream.Write(response, 0, response.Length);
                }
                else
                {
                    byte[] error = Encoding.UTF8.GetBytes("Invalid input");
                    stream.Write(error, 0, error.Length);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Client error: {e.Message}");
        }
        finally
        {
            _client.Close();
            Console.WriteLine("Client disconnected.");
        }
    }

}
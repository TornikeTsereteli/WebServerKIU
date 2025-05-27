namespace WebServerKIU.Server;

using System;
using System.Net.Sockets;
using System.Text;

public class ClientHandler
{
    private readonly TcpClient _client;

    public ClientHandler(TcpClient client)
    {
        _client = client;
    }

    public void HandleClient()
    {
        try
        {
            using NetworkStream stream = _client.GetStream();
            byte[] buffer = new byte[256];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string input = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();

            Console.WriteLine($"Received: {input}");

            string response;

            if (int.TryParse(input, out int number))
            {
                int result = number * number;
                response = result.ToString();
            }
            else
            {
                response = "Invalid number";
            }

            byte[] responseBytes = Encoding.ASCII.GetBytes(response);
            stream.Write(responseBytes, 0, responseBytes.Length);
            Console.WriteLine($"Sent: {response}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ClientHandler exception: {ex.Message}");
        }
        finally
        {
            _client.Close();
        }
    }
}

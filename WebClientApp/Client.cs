using System.Net.Sockets;
using System.Text;

namespace WebClientApp;

public class Client
{
    public static void Run()
    {
        try
        {
            // Connect to the server
            using TcpClient client = new TcpClient("127.0.0.1", 13000);
            NetworkStream stream = client.GetStream();

            Console.WriteLine("Connected to server. Type a number and press Enter (or 'exit' to quit):");

            Console.Write("Enter number: ");
            string input = Console.ReadLine();


            // Send the number to the server
            byte[] dataToSend = Encoding.ASCII.GetBytes(input);
            stream.Write(dataToSend, 0, dataToSend.Length);

            // Receive the response
            byte[] buffer = new byte[256];
            int bytes = stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.ASCII.GetString(buffer, 0, bytes);
            Console.WriteLine("Server response: " + response);
            

            client.Close();
            Console.WriteLine("Connection Closed.");
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: " + e.Message);
        }
    }
}
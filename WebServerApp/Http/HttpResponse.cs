
using System.Text;

namespace WebServerApp.Http;


public class HttpResponse
{
    public string StatusCode { get; set; } = "200 OK";
    public string ContentType { get; set; } = "text/html";
    public byte[] Content { get; set; } = Array.Empty<byte>();

    public byte[] ToBytes()
    {
        var header = $"HTTP/1.1 {StatusCode}\r\n" +
                     $"Content-Type: {ContentType}\r\n" +
                     $"Content-Length: {Content.Length}\r\n\r\n";
        var headerBytes = Encoding.UTF8.GetBytes(header);
        return headerBytes.Concat(Content).ToArray();
    }
}
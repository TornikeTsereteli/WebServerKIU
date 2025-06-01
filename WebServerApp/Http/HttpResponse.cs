
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
    
    public void WriteTo(Stream stream)
    {
        using var writer = new StreamWriter(stream, leaveOpen: true);
        writer.WriteLine($"HTTP/1.1 {StatusCode}");
        writer.WriteLine($"Content-Type: {ContentType}");
        writer.WriteLine($"Content-Length: {Content?.Length ?? 0}");
        writer.WriteLine(); // End of headers
        writer.Flush();

        if (Content != null && Content.Length > 0)
            stream.Write(Content, 0, Content.Length);
    }
}
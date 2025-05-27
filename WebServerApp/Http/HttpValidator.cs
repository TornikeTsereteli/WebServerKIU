namespace WebServerApp.Http;

public static class HttpValidator
{
    private static readonly string[] AllowedMethods = { "GET" };

    public static bool IsValid(HttpRequest request, out string error)
    {
        if (!AllowedMethods.Contains(request.Method))
        {
            error = "405 Method Not Allowed";
            return false;
        }

        if (!request.Url.StartsWith("/"))
        {
            error = "400 Bad Request";
            return false;
        }

        error = string.Empty;
        return true;
    }
}

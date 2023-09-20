using RestSharp;

namespace GitManagerLibrary.Infrastructure.Extensions;

internal static class RestSharpResponseExtension
{
    public static bool IsResponseCorrupted(this RestResponse response)
    {
        if (response is null)
            return true;

        if (response.Content is null) 
            return true;

        return false;
    }
}
namespace GitManagerLibrary.Application.Models;

public class ReturnVoid
{
    public bool IsError { get; set; }

    public string? Error { get; set; }

    public static ReturnVoid Success()
    {
        return new ReturnVoid()
        {
            IsError = false
        };
    }

    public static ReturnVoid Fail(string errorMessage)
    {
        return new ReturnVoid()
        {
            IsError = true,
            Error = errorMessage
        };
    }
}
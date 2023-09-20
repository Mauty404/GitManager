namespace GitManagerLibrary.Application.Models;

public class ReturnContent<TResult>
{
    public TResult Result { get; init; } = default!;

    public bool IsError { get; init; }

    public string Error { get; init; } = default!;

    public static ReturnContent<TResult> Success(TResult result)
    {
        return new ReturnContent<TResult>()
        {
            Result = result,
            IsError = false
        };
    }

    public static ReturnContent<TResult> Fail(string errorMessage)
    {
        return new ReturnContent<TResult>()
        {
            IsError = true,
            Error = errorMessage
        };
    }
}
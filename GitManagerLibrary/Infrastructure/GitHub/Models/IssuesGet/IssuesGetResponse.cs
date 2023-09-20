namespace GitManagerLibrary.Infrastructure.GitHub.Models.IssuesGet;

internal class IssuesGetResponse
{
    public string Title { get; init; } = default!;

    public string Body { get; init; } = default!;

    public int Number { get; init; }
}
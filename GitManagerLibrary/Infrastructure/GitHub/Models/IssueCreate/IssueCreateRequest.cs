namespace GitManagerLibrary.Infrastructure.GitHub.Models.IssueCreate;

internal class IssueCreateRequest
{
    public string Title { get; init; } = default!;

    public string Body { get; init; } = default!;
}
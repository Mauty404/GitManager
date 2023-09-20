namespace GitManagerLibrary.Infrastructure.GitHub.Models.IssueUpdate;

internal class IssueUpdateRequest
{
    public string Title { get; init; } = default!;

    public string Body { get; init; } = default!;
}
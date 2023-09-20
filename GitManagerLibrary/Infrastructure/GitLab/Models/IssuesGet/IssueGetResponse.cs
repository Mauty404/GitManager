namespace GitManagerLibrary.Infrastructure.GitLab.Models.IssuesGet;

internal class IssueGetResponse
{
    public string Title { get; init; } = default!;

    public string Description { get; init; } = default!;
}
namespace GitManagerLibrary.Infrastructure.GitLab.Models.IssueCreate;

internal class IssueCreateRequest
{
    public string Title { get; init; } = default!;

    public string Description { get; init; } = default!;
}
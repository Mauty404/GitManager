namespace GitManagerLibrary.Infrastructure.GitLab.Models.IssueUpdate;

internal class IssueUpdateRequest
{
    public string Description { get; init; } = default!;

    public string Title { get; init; } = default!;
}

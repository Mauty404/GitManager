namespace GitManagerLibrary.Application.GitLab.Models.IssueService;

public class IssueUpdateRequestDTO
{
    public int ProjectId { get; init; }

    public int IssueId { get; init; }

    public string Title { get; init; } = default!;

    public string Description { get; init; } = default!;
}
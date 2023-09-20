namespace GitManagerLibrary.Application.GitLab.Models.IssueService;

public class IssueCreateRequestDTO
{
    public int ProjectId { get; init; }

    public string Title { get; init; } = default!;

    public string Description { get; init; } = default!;
}
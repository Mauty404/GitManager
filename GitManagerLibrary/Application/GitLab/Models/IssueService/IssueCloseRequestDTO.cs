namespace GitManagerLibrary.Application.GitLab.Models.IssueService;

public class IssueCloseRequestDTO
{
    public int ProjectId { get; init; }

    public int IssueId { get; init; }
}
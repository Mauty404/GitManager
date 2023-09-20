namespace GitManagerLibrary.Application.GitHub.Models.IssueService;

public class IssueCloseRequestDTO
{
    public string RepoOwner { get; init; } = default!;

    public string RepoName { get; init; } = default!;

    public int IssueNumber { get; init; } = default!;
}
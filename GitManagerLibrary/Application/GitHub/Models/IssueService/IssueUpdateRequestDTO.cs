namespace GitManagerLibrary.Application.GitHub.Models.IssueService;

public class IssueUpdateRequestDTO
{
    public string RepoOwner { get; init; } = default!;

    public string RepoName { get; init; } = default!;

    public int IssueNumber { get; init; }

    public string IssueTitle { get; init; } = default!;

    public string IssueDescription { get; init; } = default!;
}
namespace GitManagerLibrary.Application.GitHub.Models.IssueService;

public class IssueCreateRequestDTO
{
    public string RepoOwner { get; init; } = default!;

    public string RepoName { get; init; } = default!;

    public string IssueTitle { get; init; } = default!;

    public string IssueDescription { get; init; } = default!;
}
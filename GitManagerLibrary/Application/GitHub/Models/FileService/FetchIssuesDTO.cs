namespace GitManagerLibrary.Application.GitHub.Models.FileService;

public class FetchIssuesDTO
{
    public string RepoOwner { get; init; } = default!;

    public string RepoName { get; init; } = default!;

    public string FilePath { get; init; } = default!;
}
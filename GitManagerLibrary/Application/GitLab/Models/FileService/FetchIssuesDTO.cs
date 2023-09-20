namespace GitManagerLibrary.Application.GitLab.Models.FileService;

public class FetchIssuesDTO
{
    public int ProjectId { get; init; }

    public string FilePath { get; init; } = default!;
}
namespace GitManagerLibrary.Application.GitLab.Models.FileService;

public class LoadIssuesDTO
{
    public int ProjectId { get; init; }

    public string FilePath { get; init; } = default!;
}
using GitManagerLibrary.Application.GitHub.Models.FileService;
using GitManagerLibrary.Application.Models;

namespace GitManagerLibrary.Application.GitHub.IServices;

public interface IGitHubFileService
{
    public ReturnContent<string> FetchIssuesToFile(FetchIssuesDTO request);

    public Task<ReturnContent<IEnumerable<int>>> LoadIssuesFromFile(LoadIssuesDTO request);
}

using GitManagerLibrary.Application.GitLab.Models.FileService;
using GitManagerLibrary.Application.Models;

namespace GitManagerLibrary.Application.GitLab.IServices;

public interface IGitLabFileService
{
    public ReturnContent<string> FetchIssuesToFile(FetchIssuesDTO request);

    public Task<ReturnContent<IEnumerable<int>>> LoadIssuesFromFile(LoadIssuesDTO request);
}

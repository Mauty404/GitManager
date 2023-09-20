using GitManagerLibrary.Application.GitLab.Models.IssueService;
using GitManagerLibrary.Application.Models;

namespace GitManagerLibrary.Application.GitLab.IServices;

public interface IGitLabIssueService
{
    public ReturnContent<int> CrateIssue(IssueCreateRequestDTO request);

    public ReturnVoid UpdateIssue(IssueUpdateRequestDTO request);

    public ReturnVoid CloseIssue(IssueCloseRequestDTO request);
}

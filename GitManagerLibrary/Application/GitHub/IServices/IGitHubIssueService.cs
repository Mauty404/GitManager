using GitManagerLibrary.Application.GitHub.Models.IssueService;
using GitManagerLibrary.Application.Models;

namespace GitManagerLibrary.Application.GitHub.IServices;

public interface IGitHubIssueService
{
    public ReturnContent<int> CrateIssue(IssueCreateRequestDTO request);

    public ReturnVoid UpdateIssue(IssueUpdateRequestDTO request);

    public ReturnVoid CloseIssue(IssueCloseRequestDTO request);
}

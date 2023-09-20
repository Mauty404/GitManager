using GitManagerLibrary.Application.GitHub.IServices;
using GitManagerLibrary.Application.GitHub.Models.IssueService;
using GitManagerLibrary.Application.Models;
using GitManagerLibrary.Infrastructure.GitHub.Models.IssueClose;
using GitManagerLibrary.Infrastructure.GitHub.Models.IssueCreate;
using GitManagerLibrary.Infrastructure.GitHub.Models.IssueUpdate;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace GitManagerLibrary.Infrastructure.GitHub.Services;

public class GitHubIssueService : IGitHubIssueService
{
    private readonly IConfiguration _config;
    private readonly IGitHubRestHelper _restHelper;

    public GitHubIssueService(
        IConfiguration config,
        IGitHubRestHelper restHelper)
    {
        _config = config;
        _restHelper = restHelper;
    }

    public ReturnContent<int> CrateIssue(IssueCreateRequestDTO request)
    {
        var url = $"https://api.github.com/repos/{request.RepoOwner}/{request.RepoName}/issues";

        var body = new IssueCreateRequest
        {
            Title = request.IssueTitle,
            Body = request.IssueDescription
        };

        var restResponse = _restHelper.MakeRequest(url, Method.Post, body);

        if (restResponse.IsError)
        {
            return ReturnContent<int>.Fail(restResponse.Error);
        }

        if (restResponse.Result.ResponseStatus != ResponseStatus.Completed)
        {
            return ReturnContent<int>.Fail("Request does not proceeded");
        }

        IssueCreateResponse? issue;
        try
        {
            issue = JsonConvert.DeserializeObject<IssueCreateResponse>(restResponse.Result.Content!);
        }
        catch (Exception ex)
        {
            return ReturnContent<int>.Fail(ex.Message);
        }

        return ReturnContent<int>.Success(issue!.Id);
    }


    public ReturnVoid UpdateIssue(IssueUpdateRequestDTO request)
    {
        var url = $"https://api.github.com/repos/{request.RepoOwner}/{request.RepoName}/issues/{request.IssueNumber}";

        var body = new IssueUpdateRequest
        {
            Title = request.IssueTitle,
            Body = request.IssueDescription
        };

        var restResponse = _restHelper.MakeRequest(url, Method.Patch, body);

        if (restResponse.IsError) 
        {
            return ReturnVoid.Fail(restResponse.Error);
        }

        if (restResponse.Result.StatusCode != HttpStatusCode.OK)
        {
            return ReturnVoid.Fail("Wrong response");
        }

        return ReturnVoid.Success();
    }


    public ReturnVoid CloseIssue(IssueCloseRequestDTO request)
    {
        var url = $"https://api.github.com/repos/{request.RepoOwner}/{request.RepoName}/issues/{request.IssueNumber}";

        var body = new IssueCloseRequest
        {
            State = "closed"
        };

        var restResponse = _restHelper.MakeRequest(url, Method.Patch, body);

        if (restResponse.IsError)
        {
            return ReturnVoid.Fail(restResponse.Error);
        }

        if (restResponse.Result.StatusCode != HttpStatusCode.OK)
        {
            return ReturnVoid.Fail("Wrong response");
        }

        return ReturnVoid.Success();
    }
}

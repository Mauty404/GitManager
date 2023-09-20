using GitManagerLibrary.Application.GitLab.IServices;
using GitManagerLibrary.Application.GitLab.Models.IssueService;
using GitManagerLibrary.Application.Models;
using GitManagerLibrary.Infrastructure.Extensions;
using GitManagerLibrary.Infrastructure.GitLab.Models.IssueClose;
using GitManagerLibrary.Infrastructure.GitLab.Models.IssueCreate;
using GitManagerLibrary.Infrastructure.GitLab.Models.IssueUpdate;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace GitManagerLibrary.Infrastructure.GitLab.Services;

public class GitLabIssueService : IGitLabIssueService
{
    private readonly IGitLabRestHelper _restHelper;

    public GitLabIssueService(IGitLabRestHelper restHelper)
    {
        _restHelper = restHelper;
    }


    public ReturnContent<int> CrateIssue(IssueCreateRequestDTO request)
    {
        var url = $"https://gitlab.com/api/v4/projects/{request.ProjectId}/issues";

        var body = new IssueCreateRequest
        {
            Title = request.Title,
            Description = request.Description
        };

        var restResponse = _restHelper.MakeRequest<IssueCreateRequest>(url, Method.Post, body);

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


    public ReturnVoid CloseIssue(IssueCloseRequestDTO request)
    {
        var url = $"https://gitlab.com/api/v4/projects/{request.ProjectId}/issues/{request.IssueId}";

        var restRequest = new IssueCloseRequest
        {
            state_event = "close"
        };

        var restResponse = _restHelper.MakeRequest(url, Method.Put, restRequest);

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


    public ReturnVoid UpdateIssue(IssueUpdateRequestDTO request)
    {
        var url = $"https://gitlab.com/api/v4/projects/{request.ProjectId}/issues/{request.IssueId}";

        var restRequest = new IssueUpdateRequest
        {
            Title = request.Title,
            Description = request.Description
        };

        var restResponse = _restHelper.MakeRequest(url, Method.Put, restRequest);

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
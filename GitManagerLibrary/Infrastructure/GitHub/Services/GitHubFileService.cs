using GitManagerLibrary.Application.GitHub.IServices;
using GitManagerLibrary.Application.GitHub.Models.FileService;
using GitManagerLibrary.Application.GitHub.Models.IssueService;
using GitManagerLibrary.Application.IHelpers;
using GitManagerLibrary.Application.IHelpers.Models.FileHelper;
using GitManagerLibrary.Application.Models;
using GitManagerLibrary.Infrastructure.GitHub.Models.IssuesGet;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Concurrent;
using System.Net;

namespace GitManagerLibrary.Infrastructure.GitHub.Services;

public class GitHubFileService : IGitHubFileService
{
    private readonly IGitHubRestHelper _restHelper;
    private readonly IFileHelper _fileHelper;
    private readonly IGitHubIssueService _gitHubIssueService;

    public GitHubFileService(
        IGitHubRestHelper restHelper,
        IFileHelper fileHelper,
        IGitHubIssueService gitHubIssueService)
    {
        _restHelper = restHelper;
        _fileHelper = fileHelper;
        _gitHubIssueService = gitHubIssueService;
    }

    public ReturnContent<string> FetchIssuesToFile(FetchIssuesDTO request)
    {
        var url = $"https://api.github.com/repos/{request.RepoOwner}/{request.RepoName}/issues";

        var queryParameters = new Dictionary<string, string>
        {
            { "per_page", "100" },
            { "page", "1" }
        };

        var restResponse = _restHelper.MakeRequest(url, Method.Get, queryParameters);

        if (restResponse.IsError)
        {
            return ReturnContent<string>.Fail(restResponse.Error);
        }

        if (restResponse.Result.StatusCode != HttpStatusCode.OK)
        {
            return ReturnContent<string>.Fail("Wrong response");
        }

        var restContent = restResponse.Result.Content!;
        var issues = new List<IssuesGetResponse>();

        try
        {
            issues = JsonConvert.DeserializeObject<List<IssuesGetResponse>>(restResponse.Result.Content!);
        }
        catch(Exception ex)
        {
            return ReturnContent<string>.Fail(ex.Message);
        }
        
        _fileHelper.ConvertIssuesToFile(issues!.Select(x => new Issue
        {
            Title = x.Title,
            Description = x.Body
        }),
        request.FilePath);

        return ReturnContent<string>.Success(request.FilePath);
    }

    public async Task<ReturnContent<IEnumerable<int>>> LoadIssuesFromFile(LoadIssuesDTO request)
    {
        var createdIssuesId = new ConcurrentBag<int>();

        var issues = _fileHelper.ConvertFileToIssues(request.FilePath);

        if (issues.IsError)
        {
            return ReturnContent<IEnumerable<int>>.Fail(issues.Error);
        }

        var tasks = issues.Result.Select(x =>
            Task.Run(() =>
            {
                createdIssuesId.Add(
                _gitHubIssueService.CrateIssue(new IssueCreateRequestDTO
                {
                    RepoName = request.RepoName,
                    RepoOwner = request.RepoOwner,
                    IssueTitle = x.Title,
                    IssueDescription = x.Description
                }).Result);
            }));

        await Task.WhenAll(tasks);

        return ReturnContent<IEnumerable<int>>.Success(createdIssuesId);
    }
}
using GitManagerLibrary.Application.GitLab.IServices;
using GitManagerLibrary.Application.GitLab.Models.FileService;
using RestSharp;
using Newtonsoft.Json;
using GitManagerLibrary.Infrastructure.GitLab.Models.IssuesGet;
using GitManagerLibrary.Application.IHelpers;
using GitManagerLibrary.Application.IHelpers.Models.FileHelper;
using GitManagerLibrary.Application.GitLab.Models.IssueService;
using GitManagerLibrary.Application.Models;
using System.Net;
using System.Collections.Concurrent;

namespace GitManagerLibrary.Infrastructure.GitLab.Services;

public class GitLabFileService : IGitLabFileService
{
    private readonly IGitLabRestHelper _restHelper;
    private readonly IFileHelper _fileHelper;
    private readonly IGitLabIssueService _gitLabIssueService;

    public GitLabFileService(
        IGitLabRestHelper restHelper,
        IFileHelper fileHelper,
        IGitLabIssueService gitLabIssueService)
    {
        _restHelper = restHelper;
        _fileHelper = fileHelper;
        _gitLabIssueService = gitLabIssueService;
    }

    public ReturnContent<string> FetchIssuesToFile(FetchIssuesDTO request)
    {
        var url = $"https://gitlab.com/api/v4/projects/{request.ProjectId}/issues";

        var restResponse = _restHelper.MakeRequest(url, Method.Get);

        if (restResponse.IsError) 
        {
            return ReturnContent<string>.Fail(restResponse.Error);
        }

        if (restResponse.Result.StatusCode != HttpStatusCode.OK)
        {
            return ReturnContent<string>.Fail("Wrong response");
        }

        List<IssueGetResponse>? issues;
        try
        {
            issues = JsonConvert.DeserializeObject<List<IssueGetResponse>>(restResponse.Result.Content!);
        }
        catch (Exception ex)
        {
            return ReturnContent<string>.Fail(ex.Message);
        }

        var saveToFileResult = _fileHelper.ConvertIssuesToFile(issues!.Select(x =>
            new Issue
            {
                Title = x.Title,
                Description = x.Description
            }),
        request.FilePath);

        if (saveToFileResult.IsError) 
        { 
            return ReturnContent<string>.Fail(saveToFileResult.Error!);
        }

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
                createdIssuesId.Add(_gitLabIssueService.CrateIssue(new IssueCreateRequestDTO
                {
                    ProjectId = request.ProjectId,
                    Title = x.Title,
                    Description = x.Description
                }).Result);

            }));

        await Task.WhenAll(tasks);

        return ReturnContent<IEnumerable<int>>.Success(createdIssuesId);
    }
}
using GitManager.Helpers;
using GitManagerLibrary.Application.GitLab.IServices;
using GitManagerLibrary.Application.GitLab.Models.FileService;
using GitManagerLibrary.Application.GitLab.Models.IssueService;

namespace GitManager.Providers;

internal class GitLabProvider : IProvider
{
    private readonly IGitLabIssueService _gitLabIssueService;
    private readonly IGitLabFileService _gitLabFileService;

    public GitLabProvider(
        IGitLabIssueService gitLabIssueService,
        IGitLabFileService gitLabFileService)
    {
        _gitLabIssueService = gitLabIssueService;
        _gitLabFileService = gitLabFileService;
    }

    public const string Name = "GitLab";


    public async Task Execute()
    {
        Console.Clear();
        Console.WriteLine("Choose operation:\n" +
            "1 - Create issue\n" +
            "2 - Update issue\n" +
            "3 - Close issue\n" +
            "4 - Convert issues to file\n" +
            "5 - Import issues");

        switch (Convert.ToInt32(Console.ReadLine()))
        {
            case 1:
                CreateIssue();
                break;

            case 2:
                UpdateIssue();
                break;

            case 3:
                CloseIssue();
                break;

            case 4:
                ExportIssuesToFile();
                break;

            case 5:
                await ImportIssuesFromFile();
                break;

            default:
                return;
        }
    }


    private void CreateIssue()
    {
        Console.Clear();
        Console.Write("Project ID: ");
        var projectId = Convert.ToInt32(Console.ReadLine());

        Console.Write("Title: ");
        var title = Console.ReadLine();

        Console.Write("Description: ");
        var description = Console.ReadLine();

        var result = _gitLabIssueService.CrateIssue(
            new IssueCreateRequestDTO
            {
                ProjectId = projectId,
                Title = title!,
                Description = description!
            });

        if (result.IsError)
        {
            Console.WriteLine(result.Error);
            ConsoleHelper.WaitForAction();
            return;
        }

        Console.WriteLine("Issue ID: " + result.Result);
        Console.WriteLine("ISSUE CREATED");
        ConsoleHelper.WaitForAction();
    }


    private void UpdateIssue()
    {
        Console.Clear();
        Console.Write("Project ID: ");
        var projectId = Convert.ToInt32(Console.ReadLine());

        Console.Write("Issue ID: ");
        var issueId = Convert.ToInt32(Console.ReadLine());

        Console.Write("Title: ");
        var title = Console.ReadLine();

        Console.Write("Description: ");
        var description = Console.ReadLine();

        var result = _gitLabIssueService.UpdateIssue(
            new IssueUpdateRequestDTO
            {
                IssueId = issueId,
                ProjectId = projectId,
                Title = title!,
                Description = description!
            });

        if (result.IsError)
        {
            Console.WriteLine(result.Error);
            ConsoleHelper.WaitForAction();
            return;
        }

        Console.WriteLine("ISSUE UPDATED");
        ConsoleHelper.WaitForAction();
    }


    private void CloseIssue()
    {
        Console.Clear();
        Console.Write("Project ID: ");
        var projectId = Convert.ToInt32(Console.ReadLine());

        Console.Write("Issue ID: ");
        var issueId = Convert.ToInt32(Console.ReadLine());

        var result = _gitLabIssueService.CloseIssue(
            new IssueCloseRequestDTO
            {
                ProjectId = projectId,
                IssueId = issueId
            });

        if (result.IsError)
        {
            Console.WriteLine(result.Error);
            ConsoleHelper.WaitForAction();
            return;
        }

        Console.WriteLine("ISSUE CLOSED");
        ConsoleHelper.WaitForAction();
    }


    private void ExportIssuesToFile()
    {
        Console.Clear();
        Console.Write("Project ID: ");
        var projectId = Convert.ToInt32(Console.ReadLine());

        Console.Write("Path: ");
        var path = Console.ReadLine();

        var result = _gitLabFileService.FetchIssuesToFile(
            new FetchIssuesDTO
            {
                ProjectId = projectId,
                FilePath = path!
            });

        if (result.IsError)
        {
            Console.WriteLine(result.Error);
            ConsoleHelper.WaitForAction();
            return;
        }

        Console.WriteLine("FILE SAVED: " + result.Result);
        ConsoleHelper.WaitForAction();
    }

    private async Task ImportIssuesFromFile()
    {
        Console.Clear();
        Console.Write("Project ID: ");
        var projectId = Convert.ToInt32(Console.ReadLine());

        Console.Write("Path: ");
        var path = Console.ReadLine();

        var createdIssuesResult = await _gitLabFileService.LoadIssuesFromFile(
            new LoadIssuesDTO
            {
                ProjectId = projectId,
                FilePath = path!
            });

        if (createdIssuesResult.IsError)
        {
            Console.WriteLine(createdIssuesResult.Error);
            ConsoleHelper.WaitForAction();
            return;
        }

        foreach (var createdIssueId in createdIssuesResult.Result)
        {
            Console.WriteLine("Issue ID: " + createdIssueId);
        }

        Console.WriteLine("ISSUES IMPORTED");
        ConsoleHelper.WaitForAction();
    }
}
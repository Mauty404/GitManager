using GitManager.Helpers;
using GitManagerLibrary.Application.GitHub.IServices;
using GitManagerLibrary.Application.GitHub.Models.FileService;
using GitManagerLibrary.Application.GitHub.Models.IssueService;

namespace GitManager.Providers;

internal class GitHubProvider : IProvider
{
    public GitHubProvider(
        IGitHubFileService gitHubFileService,
        IGitHubIssueService gitHubIssueService)
    {
        _gitHubFileService = gitHubFileService;
        _gitHubIssueService = gitHubIssueService;
    }

    public const string Name = "GitHub";
    private readonly IGitHubFileService _gitHubFileService;
    private readonly IGitHubIssueService _gitHubIssueService;

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
        Console.Write("Repository name: ");
        var repoName = Console.ReadLine();

        Console.Write("Repository owner: ");
        var repoOwner = Console.ReadLine();

        Console.Write("Title: ");
        var title = Console.ReadLine();

        Console.Write("Description: ");
        var description = Console.ReadLine();

        var result = _gitHubIssueService.CrateIssue(
            new IssueCreateRequestDTO
            {
                RepoName = repoName!,
                RepoOwner = repoOwner!,
                IssueTitle = title!,
                IssueDescription = description!
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
        Console.Write("Repository name: ");
        var repoName = Console.ReadLine();

        Console.Write("Repository owner: ");
        var repoOwner = Console.ReadLine();

        Console.Write("Issue ID: ");
        var issueId = Convert.ToInt32(Console.ReadLine());

        Console.Write("Title: ");
        var title = Console.ReadLine();

        Console.Write("Description: ");
        var description = Console.ReadLine();

        var result = _gitHubIssueService.UpdateIssue(
            new IssueUpdateRequestDTO
            {
                RepoName = repoName!,
                RepoOwner = repoOwner!,
                IssueNumber = issueId,
                IssueTitle = title!,
                IssueDescription = description!
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
        Console.Write("Repository name: ");
        var repoName = Console.ReadLine();

        Console.Write("Repository owner: ");
        var repoOwner = Console.ReadLine();

        Console.Write("Issue ID: ");
        var issueId = Convert.ToInt32(Console.ReadLine());

        var result = _gitHubIssueService.CloseIssue(
            new IssueCloseRequestDTO
            {
                RepoName = repoName!,
                RepoOwner = repoOwner!,
                IssueNumber = issueId
            });

        if (result.IsError)
        {
            Console.WriteLine(result.Error);
            ConsoleHelper.WaitForAction();
            return;
        }

        Console.WriteLine("ISSUE CLOSED: ");
        ConsoleHelper.WaitForAction();
    }


    private void ExportIssuesToFile()
    {
        Console.Clear();
        Console.Write("Repository name: ");
        var repoName = Console.ReadLine();

        Console.Write("Repository owner: ");
        var repoOwner = Console.ReadLine();

        Console.Write("Path: ");
        var path = Console.ReadLine();

        var result = _gitHubFileService.FetchIssuesToFile(
            new FetchIssuesDTO
            {
                RepoName = repoName!,
                RepoOwner = repoOwner!,
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
        Console.Write("Repository name: ");
        var repoName = Console.ReadLine();

        Console.Write("Repository owner: ");
        var repoOwner = Console.ReadLine();

        Console.Write("Path: ");
        var path = Console.ReadLine();

        var createdIssueIds = await _gitHubFileService.LoadIssuesFromFile(new LoadIssuesDTO
        {
            RepoName = repoName!,
            RepoOwner = repoOwner!,
            FilePath = path!
        });

        if (createdIssueIds.IsError)
        {
            Console.WriteLine(createdIssueIds.Error);
            ConsoleHelper.WaitForAction();
            return;
        }

        foreach (var createdIssueId in createdIssueIds.Result)
        {
            Console.WriteLine("Issue ID: " + createdIssueId);
        }

        Console.WriteLine("ISSUES IMPORTED");
        ConsoleHelper.WaitForAction();
    }
}
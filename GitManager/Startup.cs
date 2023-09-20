
using GitManager.Providers;

namespace GitManager.API;

internal class Startup
{
    private readonly GitLabProvider _gitLabProvider;
    private readonly GitHubProvider _gitHubProvider;

    public Startup(
        GitLabProvider gitLabProvider,
        GitHubProvider gitHubProvider)
    {
        _gitLabProvider = gitLabProvider;
        _gitHubProvider = gitHubProvider;
    }

    public async Task Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome in GitManager... \nPlease choose your git provider:");

            await ChooseGitProvider();

        }
    }

    private async Task ChooseGitProvider()
    {
        Console.WriteLine("1 - GitHub\n2 - GitLab");

        switch(Convert.ToInt32(Console.ReadLine()))
        {
            case 1:
                await _gitHubProvider.Execute();
                break;

            case 2:
                await _gitLabProvider.Execute();
                break;
        }
    }
}
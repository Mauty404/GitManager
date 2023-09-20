// See https://aka.ms/new-console-template for more information
using GitManager.API;
using GitManager.Providers;
using GitManagerLibrary.Application.GitHub.IServices;
using GitManagerLibrary.Application.GitLab.IServices;
using GitManagerLibrary.Application.IHelpers;
using GitManagerLibrary.Infrastructure.GitHub.Helpers;
using GitManagerLibrary.Infrastructure.GitHub.Services;
using GitManagerLibrary.Infrastructure.GitLab.Helpers;
using GitManagerLibrary.Infrastructure.GitLab.Services;
using GitManagerLibrary.Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

var builder = new ConfigurationBuilder();
builder
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);

builder.Build();

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((_, services) =>
    {
        services.AddTransient<IGitHubFileService, GitHubFileService>();
        services.AddTransient<IGitHubIssueService, GitHubIssueService>();
        services.AddTransient<IGitLabFileService, GitLabFileService>();
        services.AddTransient<IGitLabIssueService, GitLabIssueService>();
        services.AddTransient<IGitLabRestHelper, GitLabRestHelper>();
        services.AddTransient<IGitHubRestHelper, GitHubRestHelper>();
        services.AddTransient<IFileHelper, FileHelper>();
        services.AddTransient<GitLabProvider>();
        services.AddTransient<GitHubProvider>();
    })
    .Build();

var startup = ActivatorUtilities.CreateInstance<Startup>(host.Services);
await startup.Run();


static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
{
    var exception = e.ExceptionObject as Exception;

    Console.WriteLine("Unhandled exception");

    if (exception != null)
    {
        Console.WriteLine(exception.Message);
        Console.WriteLine("Press Enter to continue");
    }
    Console.ReadLine();
}
using GitManagerLibrary.Application.IHelpers.Models.FileHelper;
using GitManagerLibrary.Application.Models;

namespace GitManagerLibrary.Application.IHelpers;

public interface IFileHelper
{
    public ReturnVoid ConvertIssuesToFile(IEnumerable<Issue> issues, string path);

    public ReturnContent<IEnumerable<Issue>> ConvertFileToIssues(string path);
}

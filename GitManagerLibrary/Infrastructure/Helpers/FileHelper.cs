using GitManagerLibrary.Application.IHelpers;
using GitManagerLibrary.Application.IHelpers.Models.FileHelper;
using GitManagerLibrary.Application.Models;
using Newtonsoft.Json;

namespace GitManagerLibrary.Infrastructure.Helpers;

public class FileHelper : IFileHelper
{
    public ReturnContent<IEnumerable<Issue>> ConvertFileToIssues(string path)
    {
        var deserializedIssues = new List<Issue>();
        try
        {
            using var sr = new StreamReader(path);
            var json = sr.ReadToEnd();
            deserializedIssues = JsonConvert.DeserializeObject<List<Issue>>(json);
        }
        catch(Exception ex)
        {
            return ReturnContent<IEnumerable<Issue>>.Fail(ex.Message);
        }

        return ReturnContent<IEnumerable<Issue>>.Success(deserializedIssues!);
    }

    public ReturnVoid ConvertIssuesToFile(IEnumerable<Issue> issues, string path)
    {
        try
        {
            using StreamWriter writer = new StreamWriter(path, false);
            writer.Write(JsonConvert.SerializeObject(issues));
        } catch (Exception ex)
        {
            return ReturnVoid.Fail(ex.Message);
        }

        return ReturnVoid.Success();
    }
}

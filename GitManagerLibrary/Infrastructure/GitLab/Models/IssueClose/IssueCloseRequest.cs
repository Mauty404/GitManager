using Newtonsoft.Json;

namespace GitManagerLibrary.Infrastructure.GitLab.Models.IssueClose;

internal class IssueCloseRequest
{
    [JsonProperty(PropertyName = "state_event")]
    public string state_event { get; init; } = default!;
}

using GitManagerLibrary.Application.GitLab.IServices;
using GitManagerLibrary.Application.Models;
using GitManagerLibrary.Infrastructure.Extensions;
using GitManagerLibrary.Infrastructure.GitLab.Configuration;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace GitManagerLibrary.Infrastructure.GitLab.Helpers;

public class GitLabRestHelper : IGitLabRestHelper
{
    private readonly IConfiguration _config;

    public GitLabRestHelper(IConfiguration config)
    {
        _config = config;
    }

    public ReturnContent<RestResponse> MakeRequest(
        string url,
        Method method,
        Dictionary<string, string>? parameters = null)
    {
        var configuration = GetConfiguration();

        var restClient = new RestClient();
        var restRequest = new RestRequest(url);
        restRequest.AddHeader("PRIVATE-TOKEN", configuration.PersonalAccessToken);

        if (parameters != null)
        {
            foreach (var parameter in parameters)
            {
                restRequest.AddParameter(parameter.Key, parameter.Value);
            }
        }

        return Execute(restClient, restRequest, method);
    }


    public ReturnContent<RestResponse> MakeRequest<TBody>(
        string url,
        Method method,
        TBody body,
        Dictionary<string, string>? parameters = null)
        where TBody : class
    {
        var configuration = GetConfiguration();

        var restClient = new RestClient();
        var restRequest = new RestRequest(url)
        {
            RequestFormat = DataFormat.Json
        };
        restRequest.AddJsonBody(body);
        restRequest.AddHeader("PRIVATE-TOKEN", configuration.PersonalAccessToken);

        if (parameters != null)
        {
            foreach (var parameter in parameters)
            {
                restRequest.AddParameter(parameter.Key, parameter.Value);
            }
        }

        return Execute(restClient, restRequest, method);
    }

    private ReturnContent<RestResponse> Execute(RestClient restClient, RestRequest restRequest, Method method)
    {
        RestResponse? restResponse;
        try
        {
            restResponse = restClient.Execute(restRequest, method);
        }
        catch (Exception ex)
        {
            return ReturnContent<RestResponse>.Fail(ex.Message);
        }

        if (restResponse.IsResponseCorrupted())
        {
            return ReturnContent<RestResponse>.Fail("Response is corrupted");
        }

        return ReturnContent<RestResponse>.Success(restResponse);
    }

    private GitLabConfiguration GetConfiguration()
    {
        var configuration = _config.GetSection("GitLab").Get<GitLabConfiguration>();

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        return configuration;
    }
}
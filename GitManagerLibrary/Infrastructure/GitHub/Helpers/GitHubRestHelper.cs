using GitManagerLibrary.Application.GitHub.IServices;
using GitManagerLibrary.Application.Models;
using GitManagerLibrary.Infrastructure.Extensions;
using GitManagerLibrary.Infrastructure.GitHub.Configuration;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace GitManagerLibrary.Infrastructure.GitHub.Helpers;

public class GitHubRestHelper : IGitHubRestHelper
{
    private readonly IConfiguration _config;

    public GitHubRestHelper(IConfiguration config)
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
        restRequest.AddHeader("Authorization", "Bearer " + configuration.PersonalAccessToken);

        if (parameters != null)
        {
            foreach(var parameter in parameters)
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
        restRequest.AddHeader("Authorization", "Bearer " + configuration.PersonalAccessToken);

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
            return ReturnContent<RestResponse>.Fail(ex.ToString());
            //throw new Exception("Error while request to GitLab API");
        }

        if (restResponse.IsResponseCorrupted())
        {
            return ReturnContent<RestResponse>.Fail("Response from server is corrupted");
            //throw new Exception("Wrong response from rest server");
        }

        return ReturnContent<RestResponse>.Success(restResponse);
    }


    private GitHubConfiguration GetConfiguration()
    {
        var configuration = _config.GetSection("GitHub").Get<GitHubConfiguration>();

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        return configuration;
    }
}
using GitManagerLibrary.Application.Models;
using RestSharp;

namespace GitManagerLibrary.Application.IHelpers;

public interface IRestHelper
{
    public ReturnContent<RestResponse> MakeRequest(
        string url,
        Method method, 
        Dictionary<string, string>? parameters = null);

    public ReturnContent<RestResponse> MakeRequest<TBody>(
        string url, 
        Method method, 
        TBody body,
        Dictionary<string, string>? parameters = null)
        where TBody : class;
}

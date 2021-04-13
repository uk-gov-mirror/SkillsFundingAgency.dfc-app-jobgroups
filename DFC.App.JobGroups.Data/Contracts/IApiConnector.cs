using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.JobGroups.Data.Contracts
{
    public interface IApiConnector
    {
        Task<string?> GetAsync(HttpClient? httpClient, Uri url, string acceptHeader);
    }
}
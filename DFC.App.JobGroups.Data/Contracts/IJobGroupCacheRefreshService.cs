using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.JobGroups.Data.Contracts
{
    public interface IJobGroupCacheRefreshService
    {
        Task<HttpStatusCode> ReloadAsync(Uri url);

        Task<HttpStatusCode> ReloadItemAsync(Uri url);

        Task<bool> PurgeAsync();

        Task<bool> DeleteAsync(Guid contentId);
    }
}

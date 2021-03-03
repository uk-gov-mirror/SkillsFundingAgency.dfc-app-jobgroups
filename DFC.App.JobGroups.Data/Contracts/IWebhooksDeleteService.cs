using DFC.App.JobGroups.Data.Enums;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.JobGroups.Data.Contracts
{
    public interface IWebhooksDeleteService
    {
        Task<HttpStatusCode> ProcessDeleteAsync(Guid eventId, Guid contentId, MessageContentType messageContentType);

        Task<HttpStatusCode> DeleteContentAsync(Guid contentId);

        Task<HttpStatusCode> PurgeSocAsync();

        Task<HttpStatusCode> DeleteSocItemAsync(Guid contentId);
    }
}

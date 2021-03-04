using DFC.App.JobGroups.Data.Enums;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.JobGroups.Data.Contracts
{
    public interface IWebhooksService
    {
        Task<HttpStatusCode> ProcessMessageAsync(WebhookCacheOperation webhookCacheOperation, Guid eventId, Guid contentId, string? apiEndpoint);
    }
}

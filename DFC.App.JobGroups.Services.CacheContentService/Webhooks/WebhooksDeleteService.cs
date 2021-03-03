using DFC.App.JobGroups.Data;
using DFC.App.JobGroups.Data.Contracts;
using DFC.App.JobGroups.Data.Enums;
using DFC.App.JobGroups.Data.Models.CmsApiModels;
using DFC.App.JobGroups.Data.Models.ContentModels;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.JobGroups.Services.CacheContentService.Webhooks
{
    public class WebhooksDeleteService : IWebhooksDeleteService
    {
        private readonly ILogger<WebhooksDeleteService> logger;
        private readonly IDocumentService<ContentItemModel> contentItemDocumentService;
        private readonly IJobGroupCacheRefreshService jobGroupCacheRefreshService;

        public WebhooksDeleteService(
            ILogger<WebhooksDeleteService> logger,
            IDocumentService<ContentItemModel> contentItemDocumentService,
            IJobGroupCacheRefreshService jobGroupCacheRefreshService)
        {
            this.logger = logger;
            this.contentItemDocumentService = contentItemDocumentService;
            this.jobGroupCacheRefreshService = jobGroupCacheRefreshService;
        }

        public async Task<HttpStatusCode> ProcessDeleteAsync(Guid eventId, Guid contentId, MessageContentType messageContentType)
        {
            switch (messageContentType)
            {
                case MessageContentType.SharedContentItem:
                    logger.LogInformation($"Event Id: {eventId} - deleting shared content for: {contentId}");
                    return await DeleteContentAsync(contentId).ConfigureAwait(false);
                case MessageContentType.JobGroup:
                    logger.LogInformation($"Event Id: {eventId} - purging LMI SOC");
                    return await PurgeSocAsync().ConfigureAwait(false);
                case MessageContentType.JobGroupItem:
                    logger.LogInformation($"Event Id: {eventId} - deleting LMI SOC item {contentId}");
                    return await DeleteSocItemAsync(contentId).ConfigureAwait(false);
            }

            return HttpStatusCode.BadRequest;
        }

        public async Task<HttpStatusCode> DeleteContentAsync(Guid contentId)
        {
            var result = await contentItemDocumentService.DeleteAsync(contentId).ConfigureAwait(false);

            return result ? HttpStatusCode.OK : HttpStatusCode.NoContent;
        }

        public async Task<HttpStatusCode> PurgeSocAsync()
        {
            var result = await jobGroupCacheRefreshService.PurgeAsync().ConfigureAwait(false);

            return result ? HttpStatusCode.OK : HttpStatusCode.NoContent;
        }

        public async Task<HttpStatusCode> DeleteSocItemAsync(Guid contentId)
        {
            var result = await jobGroupCacheRefreshService.DeleteAsync(contentId).ConfigureAwait(false);

            return result ? HttpStatusCode.OK : HttpStatusCode.NoContent;
        }
    }
}

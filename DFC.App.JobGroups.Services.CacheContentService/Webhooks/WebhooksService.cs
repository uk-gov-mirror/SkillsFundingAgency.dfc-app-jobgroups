using DFC.App.JobGroups.Data;
using DFC.App.JobGroups.Data.Contracts;
using DFC.App.JobGroups.Data.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.JobGroups.Services.CacheContentService.Webhooks
{
    public class WebhooksService : IWebhooksService
    {
        private readonly ILogger<WebhooksService> logger;
        private readonly IWebhooksContentService webhooksContentService;
        private readonly IWebhooksDeleteService webhooksDeleteService;

        public WebhooksService(
            ILogger<WebhooksService> logger,
            IWebhooksContentService webhooksContentService,
            IWebhooksDeleteService webhooksDeleteService)
        {
            this.logger = logger;
            this.webhooksContentService = webhooksContentService;
            this.webhooksDeleteService = webhooksDeleteService;
        }

        public static MessageContentType DetermineMessageContentType(string? apiEndpoint)
        {
            if (!string.IsNullOrWhiteSpace(apiEndpoint))
            {
                if (apiEndpoint.Contains($"/{Constants.ContentTypeSharedContent.ToLowerInvariant()}/", StringComparison.OrdinalIgnoreCase))
                {
                    return MessageContentType.SharedContentItem;
                }

                if (apiEndpoint.EndsWith($"/{Constants.ApiForJobGroups}", StringComparison.OrdinalIgnoreCase))
                {
                    return MessageContentType.JobGroup;
                }

                if (apiEndpoint.Contains($"/{Constants.ApiForJobGroups}/", StringComparison.OrdinalIgnoreCase))
                {
                    return MessageContentType.JobGroupItem;
                }
            }

            return MessageContentType.None;
        }

        public async Task<HttpStatusCode> ProcessMessageAsync(bool isDraft, WebhookCacheOperation webhookCacheOperation, Guid eventId, Guid contentId, string? apiEndpoint)
        {
            var messageContentType = DetermineMessageContentType(apiEndpoint);
            if (messageContentType == MessageContentType.None)
            {
                logger.LogError($"Event Id: {eventId} got unknown message content type - {messageContentType} - {apiEndpoint}");
                return HttpStatusCode.BadRequest;
            }

            switch (webhookCacheOperation)
            {
                case WebhookCacheOperation.Delete:
                    return await webhooksDeleteService.ProcessDeleteAsync(eventId, contentId, messageContentType).ConfigureAwait(false);

                case WebhookCacheOperation.CreateOrUpdate:
                    return await webhooksContentService.ProcessContentAsync(isDraft, eventId, contentId, apiEndpoint, messageContentType).ConfigureAwait(false);
            }

            logger.LogError($"Event Id: {eventId} got unknown cache operation - {webhookCacheOperation}");
            return HttpStatusCode.BadRequest;
        }
    }
}

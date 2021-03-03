using AutoMapper;
using DFC.App.JobGroups.Data.Contracts;
using DFC.App.JobGroups.Data.Models.ContentModels;
using DFC.App.JobGroups.Services.CacheContentService.Webhooks;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;

namespace DFC.App.JobGroups.Services.CacheContentService.UnitTests.WebhooksServices
{
    public abstract class BaseWebhooksServiceTests
    {
        protected const string EventTypePublished = "published";
        protected const string EventTypeDraft = "draft";
        protected const string EventTypeDraftDiscarded = "draft-discarded";
        protected const string EventTypeDeleted = "deleted";
        protected const string EventTypeUnpublished = "unpublished";

        protected BaseWebhooksServiceTests()
        {
            FakeMapper = A.Fake<IMapper>();
            FakeWebhooksContentService = A.Fake<IWebhooksContentService>();
            FakeWebhooksDeleteService = A.Fake<IWebhooksDeleteService>();
            FakeCmsApiService = A.Fake<ICmsApiService>();
            FakeContentItemDocumentService = A.Fake<IDocumentService<ContentItemModel>>();
            FakeJobGroupCacheRefreshService = A.Fake<IJobGroupCacheRefreshService>();
        }

        protected Guid ContentIdForCreate { get; } = Guid.NewGuid();

        protected Guid ContentIdForUpdate { get; } = Guid.NewGuid();

        protected Guid ContentIdForDelete { get; } = Guid.NewGuid();

        protected Guid JobGroupIdForDelete { get; } = Guid.NewGuid();

        protected IMapper FakeMapper { get; }

        protected IWebhooksContentService FakeWebhooksContentService { get; }

        protected IWebhooksDeleteService FakeWebhooksDeleteService { get; }

        protected ICmsApiService FakeCmsApiService { get; }

        protected IDocumentService<ContentItemModel> FakeContentItemDocumentService { get; }

        protected IJobGroupCacheRefreshService FakeJobGroupCacheRefreshService { get; }

        protected WebhooksService BuildWebhooksService()
        {
            var service = new WebhooksService(A.Fake<ILogger<WebhooksService>>(), FakeWebhooksContentService, FakeWebhooksDeleteService);

            return service;
        }

        protected WebhooksContentService BuildWebhooksContentService()
        {
            var service = new WebhooksContentService(A.Fake<ILogger<WebhooksContentService>>(), FakeMapper, FakeCmsApiService, FakeContentItemDocumentService, FakeJobGroupCacheRefreshService);

            return service;
        }

        protected WebhooksDeleteService BuildWebhooksDeleteService()
        {
            var service = new WebhooksDeleteService(A.Fake<ILogger<WebhooksDeleteService>>(), FakeContentItemDocumentService, FakeJobGroupCacheRefreshService);

            return service;
        }
    }
}

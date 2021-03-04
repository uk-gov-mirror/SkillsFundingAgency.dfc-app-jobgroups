using DFC.App.JobGroups.Data.Contracts;
using DFC.App.JobGroups.Data.Enums;
using DFC.App.JobGroups.Data.Models.ContentModels;
using DFC.App.JobGroups.Services.CacheContentService.Webhooks;
using DFC.Compui.Cosmos.Contracts;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobGroups.Services.CacheContentService.UnitTests.WebhooksServices
{
    [Trait("Category", "Webhooks delete service Unit Tests")]
    public class WebhooksDeleteServiceTests
    {
        private readonly IDocumentService<ContentItemModel> fakeContentItemDocumentService = A.Fake<IDocumentService<ContentItemModel>>();

        private readonly IJobGroupCacheRefreshService fakeJobGroupCacheRefreshService = A.Fake<IJobGroupCacheRefreshService>();

        private readonly WebhooksDeleteService webhooksDeleteService;

        public WebhooksDeleteServiceTests()
        {
            webhooksDeleteService = new WebhooksDeleteService(A.Fake<ILogger<WebhooksDeleteService>>(), fakeContentItemDocumentService, fakeJobGroupCacheRefreshService);
        }

        [Fact]
        public async Task WebhooksDeleteServiceProcessDeleteForMessageContentTypeSharedContentItemReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;

            A.CallTo(() => fakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).Returns(true);

            // Act
            var result = await webhooksDeleteService.ProcessDeleteAsync(Guid.NewGuid(), Guid.NewGuid(), MessageContentType.SharedContentItem).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeJobGroupCacheRefreshService.PurgeAsync()).MustNotHaveHappened();
            A.CallTo(() => fakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServiceProcessDeleteForMessageContentTypeJobGroupReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;

            A.CallTo(() => fakeJobGroupCacheRefreshService.PurgeAsync()).Returns(true);

            // Act
            var result = await webhooksDeleteService.ProcessDeleteAsync(Guid.NewGuid(), Guid.NewGuid(), MessageContentType.JobGroup).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeJobGroupCacheRefreshService.PurgeAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServiceProcessDeleteForMessageContentTypeJobGroupItemReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;

            A.CallTo(() => fakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).Returns(true);

            // Act
            var result = await webhooksDeleteService.ProcessDeleteAsync(Guid.NewGuid(), Guid.NewGuid(), MessageContentType.JobGroupItem).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeJobGroupCacheRefreshService.PurgeAsync()).MustNotHaveHappened();
            A.CallTo(() => fakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServiceProcessDeleteForMessageContentTypeNoneReturnsBadRequest()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.BadRequest;

            // Act
            var result = await webhooksDeleteService.ProcessDeleteAsync(Guid.NewGuid(), Guid.NewGuid(), MessageContentType.None).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeJobGroupCacheRefreshService.PurgeAsync()).MustNotHaveHappened();
            A.CallTo(() => fakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServiceDeleteContentSharedContentReturnsSuccess()
        {
            // Arrange
            const bool expectedResponse = true;
            const HttpStatusCode expectedResult = HttpStatusCode.OK;

            A.CallTo(() => fakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await webhooksDeleteService.DeleteContentAsync(Guid.NewGuid()).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServiceDeleteContentSharedContentReturnsNoContent()
        {
            // Arrange
            const bool expectedResponse = false;
            const HttpStatusCode expectedResult = HttpStatusCode.NoContent;

            A.CallTo(() => fakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await webhooksDeleteService.DeleteContentAsync(Guid.NewGuid()).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServiceDeleteSocItemReturnsSuccess()
        {
            // Arrange
            const bool expectedResponse = true;
            const HttpStatusCode expectedResult = HttpStatusCode.OK;

            A.CallTo(() => fakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await webhooksDeleteService.DeleteSocItemAsync(Guid.NewGuid()).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServiceDeleteSocItemReturnsNoContent()
        {
            // Arrange
            const bool expectedResponse = false;
            const HttpStatusCode expectedResult = HttpStatusCode.NoContent;

            A.CallTo(() => fakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await webhooksDeleteService.DeleteSocItemAsync(Guid.NewGuid()).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServicePurgeSocReturnsSuccess()
        {
            // Arrange
            const bool expectedResponse = true;
            const HttpStatusCode expectedResult = HttpStatusCode.OK;

            A.CallTo(() => fakeJobGroupCacheRefreshService.PurgeAsync()).Returns(expectedResponse);

            // Act
            var result = await webhooksDeleteService.PurgeSocAsync().ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeJobGroupCacheRefreshService.PurgeAsync()).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServicePurgeSocReturnsNoContent()
        {
            // Arrange
            const bool expectedResponse = false;
            const HttpStatusCode expectedResult = HttpStatusCode.NoContent;

            A.CallTo(() => fakeJobGroupCacheRefreshService.PurgeAsync()).Returns(expectedResponse);

            // Act
            var result = await webhooksDeleteService.PurgeSocAsync().ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeJobGroupCacheRefreshService.PurgeAsync()).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }
    }
}

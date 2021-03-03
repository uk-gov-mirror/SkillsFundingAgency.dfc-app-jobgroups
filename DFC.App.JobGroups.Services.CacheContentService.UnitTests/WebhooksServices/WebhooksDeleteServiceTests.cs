using DFC.App.JobGroups.Data.Enums;
using FakeItEasy;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobGroups.Services.CacheContentService.UnitTests.WebhooksServices
{
    [Trait("Category", "Webhooks delete service Unit Tests")]
    public class WebhooksDeleteServiceTests : BaseWebhooksServiceTests
    {
        [Fact]
        public async Task WebhooksDeleteServiceProcessDeleteForMessageContentTypeSharedContentItemReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var service = BuildWebhooksDeleteService();

            A.CallTo(() => FakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).Returns(true);

            // Act
            var result = await service.ProcessDeleteAsync(Guid.NewGuid(), ContentIdForDelete, MessageContentType.SharedContentItem).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeJobGroupCacheRefreshService.PurgeAsync()).MustNotHaveHappened();
            A.CallTo(() => FakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServiceProcessDeleteForMessageContentTypeJobGroupReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var service = BuildWebhooksDeleteService();

            A.CallTo(() => FakeJobGroupCacheRefreshService.PurgeAsync()).Returns(true);

            // Act
            var result = await service.ProcessDeleteAsync(Guid.NewGuid(), JobGroupIdForDelete, MessageContentType.JobGroup).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeJobGroupCacheRefreshService.PurgeAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServiceProcessDeleteForMessageContentTypeJobGroupItemReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var service = BuildWebhooksDeleteService();

            A.CallTo(() => FakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).Returns(true);

            // Act
            var result = await service.ProcessDeleteAsync(Guid.NewGuid(), JobGroupIdForDelete, MessageContentType.JobGroupItem).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeJobGroupCacheRefreshService.PurgeAsync()).MustNotHaveHappened();
            A.CallTo(() => FakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServiceProcessDeleteForMessageContentTypeNoneReturnsBadRequest()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.BadRequest;
            var service = BuildWebhooksDeleteService();

            // Act
            var result = await service.ProcessDeleteAsync(Guid.NewGuid(), ContentIdForDelete, MessageContentType.None).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeJobGroupCacheRefreshService.PurgeAsync()).MustNotHaveHappened();
            A.CallTo(() => FakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServiceDeleteContentSharedContentReturnsSuccess()
        {
            // Arrange
            const bool expectedResponse = true;
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var service = BuildWebhooksDeleteService();

            A.CallTo(() => FakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await service.DeleteContentAsync(ContentIdForDelete).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServiceDeleteContentSharedContentReturnsNoContent()
        {
            // Arrange
            const bool expectedResponse = false;
            const HttpStatusCode expectedResult = HttpStatusCode.NoContent;
            var service = BuildWebhooksDeleteService();

            A.CallTo(() => FakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await service.DeleteContentAsync(ContentIdForDelete).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServiceDeleteSocItemReturnsSuccess()
        {
            // Arrange
            const bool expectedResponse = true;
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var service = BuildWebhooksDeleteService();

            A.CallTo(() => FakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await service.DeleteSocItemAsync(JobGroupIdForDelete).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServiceDeleteSocItemReturnsNoContent()
        {
            // Arrange
            const bool expectedResponse = false;
            const HttpStatusCode expectedResult = HttpStatusCode.NoContent;
            var service = BuildWebhooksDeleteService();

            A.CallTo(() => FakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await service.DeleteSocItemAsync(JobGroupIdForDelete).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupCacheRefreshService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServicePurgeSocReturnsSuccess()
        {
            // Arrange
            const bool expectedResponse = true;
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var service = BuildWebhooksDeleteService();

            A.CallTo(() => FakeJobGroupCacheRefreshService.PurgeAsync()).Returns(expectedResponse);

            // Act
            var result = await service.PurgeSocAsync().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupCacheRefreshService.PurgeAsync()).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksDeleteServicePurgeSocReturnsNoContent()
        {
            // Arrange
            const bool expectedResponse = false;
            const HttpStatusCode expectedResult = HttpStatusCode.NoContent;
            var service = BuildWebhooksDeleteService();

            A.CallTo(() => FakeJobGroupCacheRefreshService.PurgeAsync()).Returns(expectedResponse);

            // Act
            var result = await service.PurgeSocAsync().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupCacheRefreshService.PurgeAsync()).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }
    }
}

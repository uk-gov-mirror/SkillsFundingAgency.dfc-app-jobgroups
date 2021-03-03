using DFC.App.JobGroups.Data.Enums;
using DFC.App.JobGroups.Data.Models.CmsApiModels;
using DFC.App.JobGroups.Data.Models.ContentModels;
using FakeItEasy;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobGroups.Services.CacheContentService.UnitTests.WebhooksServices
{
    [Trait("Category", "Webhooks content service  Unit Tests")]
    public class WebhooksContentServiceTests : BaseWebhooksServiceTests
    {
        [Fact]
        public async Task WebhooksContentServiceProcessContentForSharedContentItemReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var service = BuildWebhooksContentService();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).Returns(A.Dummy<CmsApiSharedContentModel>());
            A.CallTo(() => FakeMapper.Map<ContentItemModel>(A<CmsApiSharedContentModel>.Ignored)).Returns(A.Dummy<ContentItemModel>());
            A.CallTo(() => FakeContentItemDocumentService.UpsertAsync(A<ContentItemModel>.Ignored)).Returns(expectedResult);

            // Act
            var result = await service.ProcessContentAsync(Guid.NewGuid(), "https://somewhere.com", MessageContentType.SharedContentItem).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupCacheRefreshService.ReloadAsync(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeJobGroupCacheRefreshService.ReloadItemAsync(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<ContentItemModel>(A<CmsApiSharedContentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeContentItemDocumentService.UpsertAsync(A<ContentItemModel>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksContentServiceProcessContentForJobGroupReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var service = BuildWebhooksContentService();

            A.CallTo(() => FakeJobGroupCacheRefreshService.ReloadAsync(A<Uri>.Ignored)).Returns(expectedResult);

            // Act
            var result = await service.ProcessContentAsync(Guid.NewGuid(), "https://somewhere.com", MessageContentType.JobGroup).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupCacheRefreshService.ReloadAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeJobGroupCacheRefreshService.ReloadItemAsync(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeMapper.Map<ContentItemModel>(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeContentItemDocumentService.UpsertAsync(A<ContentItemModel>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksContentServiceProcessContentForJobGroupItemReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var service = BuildWebhooksContentService();

            A.CallTo(() => FakeJobGroupCacheRefreshService.ReloadItemAsync(A<Uri>.Ignored)).Returns(expectedResult);

            // Act
            var result = await service.ProcessContentAsync(Guid.NewGuid(), "https://somewhere.com", MessageContentType.JobGroupItem).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupCacheRefreshService.ReloadAsync(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeJobGroupCacheRefreshService.ReloadItemAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeMapper.Map<ContentItemModel>(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeContentItemDocumentService.UpsertAsync(A<ContentItemModel>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksContentServiceProcessContentForJobGroupItemReturnsBadRequest()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.BadRequest;
            var service = BuildWebhooksContentService();

            // Act
            var result = await service.ProcessContentAsync(Guid.NewGuid(), "https://somewhere.com", MessageContentType.None).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupCacheRefreshService.ReloadAsync(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeJobGroupCacheRefreshService.ReloadItemAsync(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeMapper.Map<ContentItemModel>(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeContentItemDocumentService.UpsertAsync(A<ContentItemModel>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksContentServiceProcessContentForBadUriReturnsInvalidDataException()
        {
            // Arrange
            const string apiEndpoint = "https//:somewhere.com";
            var eventId = Guid.NewGuid();
            var service = BuildWebhooksContentService();

            // Act
            var exceptionResult = await Assert.ThrowsAsync<InvalidDataException>(async () => await service.ProcessContentAsync(eventId, apiEndpoint, MessageContentType.None).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            A.CallTo(() => FakeJobGroupCacheRefreshService.ReloadAsync(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeJobGroupCacheRefreshService.ReloadItemAsync(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeMapper.Map<ContentItemModel>(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeContentItemDocumentService.UpsertAsync(A<ContentItemModel>.Ignored)).MustNotHaveHappened();
            Assert.Equal($"Invalid Api url '{apiEndpoint}' received for Event Id: {eventId}", exceptionResult.Message);
        }

        [Fact]
        public async Task WebhooksContentServiceProcessSharedContentForCreateReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var service = BuildWebhooksContentService();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).Returns(A.Dummy<CmsApiSharedContentModel>());
            A.CallTo(() => FakeMapper.Map<ContentItemModel>(A<CmsApiSharedContentModel>.Ignored)).Returns(A.Dummy<ContentItemModel>());
            A.CallTo(() => FakeContentItemDocumentService.UpsertAsync(A<ContentItemModel>.Ignored)).Returns(expectedResult);

            // Act
            var result = await service.ProcessSharedContentAsync(ContentIdForCreate, new Uri("https://somewhere.com", UriKind.Absolute)).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<ContentItemModel>(A<CmsApiSharedContentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeContentItemDocumentService.UpsertAsync(A<ContentItemModel>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksContentServiceProcessSharedContentForCreateReturnsNoContent()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.NoContent;
            ContentItemModel? nullContentItemModel = null;
            var service = BuildWebhooksContentService();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).Returns(A.Dummy<CmsApiSharedContentModel>());
            A.CallTo(() => FakeMapper.Map<ContentItemModel?>(A<CmsApiSharedContentModel>.Ignored)).Returns(nullContentItemModel);
            A.CallTo(() => FakeContentItemDocumentService.UpsertAsync(A<ContentItemModel>.Ignored)).Returns(expectedResult);

            // Act
            var result = await service.ProcessSharedContentAsync(ContentIdForCreate, new Uri("https://somewhere.com", UriKind.Absolute)).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<ContentItemModel>(A<CmsApiSharedContentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeContentItemDocumentService.UpsertAsync(A<ContentItemModel>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }
    }
}

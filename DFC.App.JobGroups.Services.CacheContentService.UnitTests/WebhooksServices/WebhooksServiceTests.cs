using DFC.App.JobGroups.Data;
using DFC.App.JobGroups.Data.Enums;
using DFC.App.JobGroups.Services.CacheContentService.Webhooks;
using FakeItEasy;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobGroups.Services.CacheContentService.UnitTests.WebhooksServices
{
    [Trait("Category", "Webhooks service  Unit Tests")]
    public class WebhooksServiceTests : BaseWebhooksServiceTests
    {
        [Theory]
        [InlineData(null, MessageContentType.None)]
        [InlineData("", MessageContentType.None)]
        [InlineData("https://someehwre.com/api/sharedcontent/" + Constants.SharedContentAskAdviserItemId, MessageContentType.SharedContentItem)]
        [InlineData("https://someehwre.com/api/" + Constants.ApiForJobGroups, MessageContentType.JobGroup)]
        [InlineData("https://someehwre.com/api/" + Constants.ApiForJobGroups + "/", MessageContentType.JobGroupItem)]
        public void WebhooksServiceDetermineMessageContentTypeReturnsExpected(string? apiEndpoint, MessageContentType expectedResponse)
        {
            // Arrange

            // Act
            var result = WebhooksService.DetermineMessageContentType(apiEndpoint);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageForDeleteReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var apiEndpoint = $"https://somewhere.com/api/{Constants.ApiForJobGroups}";
            var service = BuildWebhooksService();

            A.CallTo(() => FakeWebhooksDeleteService.ProcessDeleteAsync(A<Guid>.Ignored, A<Guid>.Ignored, A<MessageContentType>.Ignored)).Returns(expectedResult);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.Delete, Guid.NewGuid(), ContentIdForDelete, apiEndpoint).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhooksDeleteService.ProcessDeleteAsync(A<Guid>.Ignored, A<Guid>.Ignored, A<MessageContentType>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeWebhooksContentService.ProcessContentAsync(A<Guid>.Ignored, A<string>.Ignored, A<MessageContentType>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageForCreateOrUpdateReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var apiEndpoint = $"https://somewhere.com/api/{Constants.ApiForJobGroups}";
            var service = BuildWebhooksService();

            A.CallTo(() => FakeWebhooksContentService.ProcessContentAsync(A<Guid>.Ignored, A<string>.Ignored, A<MessageContentType>.Ignored)).Returns(expectedResult);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), ContentIdForDelete, apiEndpoint).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhooksDeleteService.ProcessDeleteAsync(A<Guid>.Ignored, A<Guid>.Ignored, A<MessageContentType>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeWebhooksContentService.ProcessContentAsync(A<Guid>.Ignored, A<string>.Ignored, A<MessageContentType>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageForNoneReturnsBadRequest()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.BadRequest;
            var apiEndpoint = $"https://somewhere.com/api/{Constants.ApiForJobGroups}";
            var service = BuildWebhooksService();

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.None, Guid.NewGuid(), ContentIdForDelete, apiEndpoint).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhooksDeleteService.ProcessDeleteAsync(A<Guid>.Ignored, A<Guid>.Ignored, A<MessageContentType>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeWebhooksContentService.ProcessContentAsync(A<Guid>.Ignored, A<string>.Ignored, A<MessageContentType>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageForBadMessageContentTypeReturnsBadRequest()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.BadRequest;
            const string apiEndpoint = "https://somewhere.com/api/";
            var service = BuildWebhooksService();

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), ContentIdForDelete, apiEndpoint).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhooksDeleteService.ProcessDeleteAsync(A<Guid>.Ignored, A<Guid>.Ignored, A<MessageContentType>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeWebhooksContentService.ProcessContentAsync(A<Guid>.Ignored, A<string>.Ignored, A<MessageContentType>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }
    }
}

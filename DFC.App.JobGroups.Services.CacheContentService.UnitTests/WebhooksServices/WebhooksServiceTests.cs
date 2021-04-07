using DFC.App.JobGroups.Data;
using DFC.App.JobGroups.Data.Contracts;
using DFC.App.JobGroups.Data.Enums;
using DFC.App.JobGroups.Services.CacheContentService.Webhooks;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobGroups.Services.CacheContentService.UnitTests.WebhooksServices
{
    [Trait("Category", "Webhooks service  Unit Tests")]
    public class WebhooksServiceTests
    {
        private readonly IWebhooksContentService fakeWebhooksContentService = A.Fake<IWebhooksContentService>();

        private readonly IWebhooksDeleteService fakeWebhooksDeleteService = A.Fake<IWebhooksDeleteService>();

        private readonly WebhooksService webhooksService;

        public WebhooksServiceTests()
        {
            webhooksService = new WebhooksService(A.Fake<ILogger<WebhooksService>>(), fakeWebhooksContentService, fakeWebhooksDeleteService);
        }

        [Theory]
        [InlineData(null, MessageContentType.None)]
        [InlineData("", MessageContentType.None)]
        [InlineData("https://somewhere.com/api/sharedcontent/" + Constants.SharedContentAskAdviserItemId, MessageContentType.SharedContentItem)]
        [InlineData("https://somewhere.com/api/" + Constants.ApiForJobGroups, MessageContentType.JobGroup)]
        [InlineData("https://somewhere.com/api/" + Constants.ApiForJobGroups + "/", MessageContentType.JobGroupItem)]
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

            A.CallTo(() => fakeWebhooksDeleteService.ProcessDeleteAsync(A<Guid>.Ignored, A<Guid>.Ignored, A<MessageContentType>.Ignored)).Returns(expectedResult);

            // Act
            var result = await webhooksService.ProcessMessageAsync(true, WebhookCacheOperation.Delete, Guid.NewGuid(), Guid.NewGuid(), apiEndpoint).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeWebhooksDeleteService.ProcessDeleteAsync(A<Guid>.Ignored, A<Guid>.Ignored, A<MessageContentType>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeWebhooksContentService.ProcessContentAsync(A<bool>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<string>.Ignored, A<MessageContentType>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageForCreateOrUpdateReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var apiEndpoint = $"https://somewhere.com/api/{Constants.ApiForJobGroups}";

            A.CallTo(() => fakeWebhooksContentService.ProcessContentAsync(A<bool>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<string>.Ignored, A<MessageContentType>.Ignored)).Returns(expectedResult);

            // Act
            var result = await webhooksService.ProcessMessageAsync(true, WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), Guid.NewGuid(), apiEndpoint).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeWebhooksDeleteService.ProcessDeleteAsync(A<Guid>.Ignored, A<Guid>.Ignored, A<MessageContentType>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeWebhooksContentService.ProcessContentAsync(A<bool>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<string>.Ignored, A<MessageContentType>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageForNoneReturnsBadRequest()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.BadRequest;
            var apiEndpoint = $"https://somewhere.com/api/{Constants.ApiForJobGroups}";

            // Act
            var result = await webhooksService.ProcessMessageAsync(true, WebhookCacheOperation.None, Guid.NewGuid(), Guid.NewGuid(), apiEndpoint).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeWebhooksDeleteService.ProcessDeleteAsync(A<Guid>.Ignored, A<Guid>.Ignored, A<MessageContentType>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeWebhooksContentService.ProcessContentAsync(A<bool>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<string>.Ignored, A<MessageContentType>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageForBadMessageContentTypeReturnsBadRequest()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.BadRequest;
            const string apiEndpoint = "https://somewhere.com/api/";

            // Act
            var result = await webhooksService.ProcessMessageAsync(true, WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), Guid.NewGuid(), apiEndpoint).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeWebhooksDeleteService.ProcessDeleteAsync(A<Guid>.Ignored, A<Guid>.Ignored, A<MessageContentType>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeWebhooksContentService.ProcessContentAsync(A<bool>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<string>.Ignored, A<MessageContentType>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }
    }
}

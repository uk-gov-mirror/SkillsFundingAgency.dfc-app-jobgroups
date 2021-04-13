using DFC.App.JobGroups.Data.Contracts;
using DFC.App.JobGroups.Data.Models;
using DFC.App.JobGroups.Data.Models.JobGroupModels;
using DFC.Compui.Cosmos.Contracts;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobGroups.Services.CacheContentService.UnitTests
{
    [Trait("Category", "Job Group draft cache refresh Unit Tests")]
    public class JobGroupCacheRefreshServiceTests
    {
        private readonly ILogger<JobGroupCacheRefreshService> fakeLogger = A.Fake<ILogger<JobGroupCacheRefreshService>>();
        private readonly IDocumentService<JobGroupModel> fakeJobGroupDocumentService = A.Fake<IDocumentService<JobGroupModel>>();
        private readonly ILmiTransformationApiConnector fakeLmiTransformationApiConnector = A.Fake<ILmiTransformationApiConnector>();
        private readonly JobGroupCacheRefreshService jobGroupCacheRefreshService;

        public JobGroupCacheRefreshServiceTests()
        {
            jobGroupCacheRefreshService = new JobGroupCacheRefreshService(fakeLogger, fakeJobGroupDocumentService, fakeLmiTransformationApiConnector);
        }

        [Fact]
        public async Task JobGroupCacheRefreshServiceReloadIsSuccessful()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var existingJobGroup = A.Dummy<JobGroupModel>();
            var getSummaryResponse = new List<JobGroupSummaryItemModel>
            {
                 new JobGroupSummaryItemModel
                 {
                     Soc = 1,
                     Title = "A title 1",
                 },
                 new JobGroupSummaryItemModel
                 {
                     Soc = 2,
                     Title = "A title 2",
                 },
            };
            var getDetailResponse = new JobGroupModel
            {
                Soc = 2,
                Title = "A title 2",
            };

            A.CallTo(() => fakeLmiTransformationApiConnector.GetSummaryAsync(A<Uri>.Ignored)).Returns(getSummaryResponse);
            A.CallTo(() => fakeJobGroupDocumentService.PurgeAsync()).Returns(true);
            A.CallTo(() => fakeLmiTransformationApiConnector.GetDetailsAsync(A<Uri>.Ignored)).Returns(getDetailResponse);
            A.CallTo(() => fakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).Returns(existingJobGroup);
            A.CallTo(() => fakeJobGroupDocumentService.UpsertAsync(A<JobGroupModel>.Ignored)).Returns(HttpStatusCode.OK);

            // act
            var result = await jobGroupCacheRefreshService.ReloadAsync(new Uri("https://somewhere.com", UriKind.Absolute)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeLmiTransformationApiConnector.GetSummaryAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeJobGroupDocumentService.PurgeAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeLmiTransformationApiConnector.GetDetailsAsync(A<Uri>.Ignored)).MustHaveHappened(getSummaryResponse.Count, Times.Exactly);
            A.CallTo(() => fakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).MustHaveHappened(getSummaryResponse.Count, Times.Exactly);
            A.CallTo(() => fakeJobGroupDocumentService.UpsertAsync(A<JobGroupModel>.Ignored)).MustHaveHappened(getSummaryResponse.Count, Times.Exactly);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task JobGroupCacheRefreshServiceReloadReturnsNoSummaries()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.NoContent;
            var noModels = A.CollectionOfDummy<JobGroupSummaryItemModel>(0);

            A.CallTo(() => fakeLmiTransformationApiConnector.GetSummaryAsync(A<Uri>.Ignored)).Returns(noModels);

            // act
            var result = await jobGroupCacheRefreshService.ReloadAsync(new Uri("https://somewhere.com", UriKind.Absolute)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeLmiTransformationApiConnector.GetSummaryAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeJobGroupDocumentService.PurgeAsync()).MustNotHaveHappened();
            A.CallTo(() => fakeLmiTransformationApiConnector.GetDetailsAsync(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeJobGroupDocumentService.UpsertAsync(A<JobGroupModel>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task JobGroupCacheRefreshServiceReloadReturnsNullForSummaries()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.NoContent;
            IList<JobGroupSummaryItemModel>? nullModels = default;

            A.CallTo(() => fakeLmiTransformationApiConnector.GetSummaryAsync(A<Uri>.Ignored)).Returns(nullModels);

            // act
            var result = await jobGroupCacheRefreshService.ReloadAsync(new Uri("https://somewhere.com", UriKind.Absolute)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeLmiTransformationApiConnector.GetSummaryAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeJobGroupDocumentService.PurgeAsync()).MustNotHaveHappened();
            A.CallTo(() => fakeLmiTransformationApiConnector.GetDetailsAsync(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeJobGroupDocumentService.UpsertAsync(A<JobGroupModel>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task JobGroupCacheRefreshServiceReloadItemForExistingJobGroupIsSuccessful()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var existingJobGroup = A.Dummy<JobGroupModel>();
            var getDetailResponse = new JobGroupModel
            {
                Soc = 2,
                Title = "A title 2",
            };

            A.CallTo(() => fakeLmiTransformationApiConnector.GetDetailsAsync(A<Uri>.Ignored)).Returns(getDetailResponse);
            A.CallTo(() => fakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).Returns(existingJobGroup);
            A.CallTo(() => fakeJobGroupDocumentService.UpsertAsync(A<JobGroupModel>.Ignored)).Returns(HttpStatusCode.OK);

            // act
            var result = await jobGroupCacheRefreshService.ReloadItemAsync(new Uri("https://somewhere.com", UriKind.Absolute)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeLmiTransformationApiConnector.GetDetailsAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeJobGroupDocumentService.UpsertAsync(A<JobGroupModel>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task JobGroupCacheRefreshServiceReloadItemForNonExistingJobGroupIsSuccessful()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            JobGroupModel? nullJobGroup = null;
            var getDetailResponse = new JobGroupModel
            {
                Soc = 2,
                Title = "A title 2",
            };

            A.CallTo(() => fakeLmiTransformationApiConnector.GetDetailsAsync(A<Uri>.Ignored)).Returns(getDetailResponse);
            A.CallTo(() => fakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).Returns(nullJobGroup);
            A.CallTo(() => fakeJobGroupDocumentService.UpsertAsync(A<JobGroupModel>.Ignored)).Returns(HttpStatusCode.OK);

            // act
            var result = await jobGroupCacheRefreshService.ReloadItemAsync(new Uri("https://somewhere.com", UriKind.Absolute)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeLmiTransformationApiConnector.GetDetailsAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeJobGroupDocumentService.UpsertAsync(A<JobGroupModel>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task JobGroupCacheRefreshServiceReloadItemReturnsBadRequest()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.BadRequest;
            JobGroupModel? getDetailResponse = null;

            A.CallTo(() => fakeLmiTransformationApiConnector.GetDetailsAsync(A<Uri>.Ignored)).Returns(getDetailResponse);

            // act
            var result = await jobGroupCacheRefreshService.ReloadItemAsync(new Uri("https://somewhere.com", UriKind.Absolute)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeLmiTransformationApiConnector.GetDetailsAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeJobGroupDocumentService.UpsertAsync(A<JobGroupModel>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task JobGroupCacheRefreshServicePurgeIsSuccessful()
        {
            // arrange
            const bool expectedResult = true;
            A.CallTo(() => fakeJobGroupDocumentService.PurgeAsync()).Returns(true);

            // act
            var result = await jobGroupCacheRefreshService.PurgeAsync().ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeJobGroupDocumentService.PurgeAsync()).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task JobGroupCacheRefreshServiceDeleteIsSuccessful()
        {
            // arrange
            const bool expectedResult = true;
            A.CallTo(() => fakeJobGroupDocumentService.DeleteAsync(A<Guid>.Ignored)).Returns(true);

            // act
            var result = await jobGroupCacheRefreshService.DeleteAsync(Guid.NewGuid()).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeJobGroupDocumentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }
    }
}

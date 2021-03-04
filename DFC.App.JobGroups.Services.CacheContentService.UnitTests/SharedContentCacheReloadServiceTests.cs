using AutoMapper;
using DFC.App.JobGroups.Data;
using DFC.App.JobGroups.Data.Models.CmsApiModels;
using DFC.App.JobGroups.Data.Models.ContentModels;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobGroups.Services.CacheContentService.UnitTests
{
    [Trait("Category", "Shared content cache refresh Unit Tests")]
    public class SharedContentCacheReloadServiceTests
    {
        private readonly ILogger<SharedContentCacheReloadService> fakeLogger = A.Fake<ILogger<SharedContentCacheReloadService>>();
        private readonly IMapper fakeMapper = A.Fake<IMapper>();
        private readonly IDocumentService<ContentItemModel> fakeContentItemDocumentService = A.Fake<IDocumentService<ContentItemModel>>();
        private readonly ICmsApiService fakeCmsApiService = A.Fake<ICmsApiService>();
        private readonly CmsApiClientOptions cmsApiClientOptions = new CmsApiClientOptions { BaseAddress = new Uri("https://somewhere.com", UriKind.Absolute) };
        private readonly IContentTypeMappingService fakeContentTypeMappingService = A.Fake<IContentTypeMappingService>();
        private readonly SharedContentCacheReloadService sharedContentCacheReloadService;

        public SharedContentCacheReloadServiceTests()
        {
            sharedContentCacheReloadService = new SharedContentCacheReloadService(fakeLogger, fakeMapper, fakeContentItemDocumentService, fakeCmsApiService, cmsApiClientOptions, fakeContentTypeMappingService);
        }

        [Fact]
        public async Task JobGroupCacheRefreshServiceReloadIsSuccessful()
        {
            // arrange
            const HttpStatusCode expectedUpsertResult = HttpStatusCode.OK;

            A.CallTo(() => fakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).Returns(A.Dummy<CmsApiSharedContentModel>());
            A.CallTo(() => fakeMapper.Map<ContentItemModel>(A<CmsApiSharedContentModel>.Ignored)).Returns(A.Dummy<ContentItemModel>());
            A.CallTo(() => fakeContentItemDocumentService.UpsertAsync(A<ContentItemModel>.Ignored)).Returns(expectedUpsertResult);

            // act
            await sharedContentCacheReloadService.ReloadAsync(new CancellationToken(false)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentTypeMappingService.AddMapping(A<string>.Ignored, A<Type>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeMapper.Map<ContentItemModel>(A<CmsApiSharedContentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeContentItemDocumentService.UpsertAsync(A<ContentItemModel>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task JobGroupCacheRefreshServiceReloadForNoData()
        {
            // arrange
            CmsApiSharedContentModel? nullCmsApiSharedContentModel = null;
            A.CallTo(() => fakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).Returns(nullCmsApiSharedContentModel);

            // act
            await sharedContentCacheReloadService.ReloadAsync(new CancellationToken(false)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentTypeMappingService.AddMapping(A<string>.Ignored, A<Type>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeMapper.Map<ContentItemModel>(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeContentItemDocumentService.UpsertAsync(A<ContentItemModel>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task JobGroupCacheRefreshServiceReloadIsCancelled()
        {
            // arrange

            // act
            await sharedContentCacheReloadService.ReloadAsync(new CancellationToken(true)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentTypeMappingService.AddMapping(A<string>.Ignored, A<Type>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}

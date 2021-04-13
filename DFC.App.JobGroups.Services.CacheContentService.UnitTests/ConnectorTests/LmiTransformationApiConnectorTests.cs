using DFC.App.JobGroups.Data.Contracts;
using DFC.App.JobGroups.Data.Models;
using DFC.App.JobGroups.Data.Models.JobGroupModels;
using DFC.App.JobGroups.Services.CacheContentService.Connectors;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobGroups.Services.CacheContentService.UnitTests.ConnectorTests
{
    [Trait("Category", "LMI Transformation API connector Unit Tests")]
    public class LmiTransformationApiConnectorTests
    {
        private readonly ILogger<LmiTransformationApiConnector> fakeLogger = A.Fake<ILogger<LmiTransformationApiConnector>>();
        private readonly HttpClient httpClient = new HttpClient();
        private readonly IApiDataConnector fakeApiDataConnector = A.Fake<IApiDataConnector>();
        private readonly ILmiTransformationApiConnector lmiTransformationApiConnector;

        public LmiTransformationApiConnectorTests()
        {
            lmiTransformationApiConnector = new LmiTransformationApiConnector(fakeLogger, httpClient, fakeApiDataConnector);
        }

        [Fact]
        public async Task LmiTransformationApiConnectorTestsGetSummaryReturnsSuccess()
        {
            // arrange
            var expectedResults = new List<JobGroupSummaryItemModel>
            {
                new JobGroupSummaryItemModel
                {
                    Soc = 3543,
                    Title = "A title",
                },
            };

            A.CallTo(() => fakeApiDataConnector.GetAsync<IList<JobGroupSummaryItemModel>>(A<HttpClient>.Ignored, A<Uri>.Ignored)).Returns(expectedResults);

            // act
            var results = await lmiTransformationApiConnector.GetSummaryAsync(new Uri("https://somewhere.com", UriKind.Absolute)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiDataConnector.GetAsync<IList<JobGroupSummaryItemModel>>(A<HttpClient>.Ignored, A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.NotNull(results);
            Assert.Equal(expectedResults.Count, results!.Count);
            Assert.Equal(expectedResults.First().Soc, results.First().Soc);
            Assert.Equal(expectedResults.First().Title, results.First().Title);
        }

        [Fact]
        public async Task LmiTransformationApiConnectorTestsGetDetailsReturnsSuccess()
        {
            // arrange
            var expectedResult = new JobGroupModel
            {
                Soc = 3543,
                Title = "A title",
            };

            A.CallTo(() => fakeApiDataConnector.GetAsync<JobGroupModel>(A<HttpClient>.Ignored, A<Uri>.Ignored)).Returns(expectedResult);

            // act
            var result = await lmiTransformationApiConnector.GetDetailsAsync(new Uri("https://somewhere.com", UriKind.Absolute)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiDataConnector.GetAsync<JobGroupModel>(A<HttpClient>.Ignored, A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Soc, result?.Soc);
            Assert.Equal(expectedResult.Title, result?.Title);
        }
    }
}

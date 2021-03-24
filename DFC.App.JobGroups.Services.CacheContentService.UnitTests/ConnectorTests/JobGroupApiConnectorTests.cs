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
    [Trait("Category", "Job group API connector Unit Tests")]
    public class JobGroupApiConnectorTests
    {
        private readonly ILogger<JobGroupApiConnector> fakeLogger = A.Fake<ILogger<JobGroupApiConnector>>();
        private readonly HttpClient httpClient = new HttpClient();
        private readonly IApiDataConnector fakeApiDataConnector = A.Fake<IApiDataConnector>();
        private readonly IJobGroupApiConnector jobGroupApiConnector;

        public JobGroupApiConnectorTests()
        {
            jobGroupApiConnector = new JobGroupApiConnector(fakeLogger, httpClient, fakeApiDataConnector);
        }

        [Fact]
        public async Task JobGroupApiConnectorTestsGetSummaryReturnsSuccess()
        {
            // arrange
            var expectedResults = new List<JobGroupSummaryItemModel>
            {
                new JobGroupSummaryItemModel
                {
                    Id = Guid.NewGuid(),
                    Soc = 3543,
                    Title = "A title",
                },
            };

            A.CallTo(() => fakeApiDataConnector.GetAsync<IList<JobGroupSummaryItemModel>>(A<HttpClient>.Ignored, A<Uri>.Ignored)).Returns(expectedResults);

            // act
            var results = await jobGroupApiConnector.GetSummaryAsync(new Uri("https://somewhere.com", UriKind.Absolute)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiDataConnector.GetAsync<IList<JobGroupSummaryItemModel>>(A<HttpClient>.Ignored, A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.NotNull(results);
            Assert.Equal(expectedResults.Count, results!.Count);
            Assert.Equal(expectedResults.First().Soc, results.First().Soc);
            Assert.Equal(expectedResults.First().Title, results.First().Title);
        }

        [Fact]
        public async Task JobGroupApiConnectorTestsGetDetailsReturnsSuccess()
        {
            // arrange
            var expectedResult = new JobGroupModel
            {
                Id = Guid.NewGuid(),
                Soc = 3543,
                Title = "A title",
            };

            A.CallTo(() => fakeApiDataConnector.GetAsync<JobGroupModel>(A<HttpClient>.Ignored, A<Uri>.Ignored)).Returns(expectedResult);

            // act
            var result = await jobGroupApiConnector.GetDetailsAsync(new Uri("https://somewhere.com", UriKind.Absolute)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiDataConnector.GetAsync<JobGroupModel>(A<HttpClient>.Ignored, A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Soc, result?.Soc);
            Assert.Equal(expectedResult.Title, result?.Title);
        }
    }
}

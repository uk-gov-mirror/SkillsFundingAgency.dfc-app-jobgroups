using DFC.App.JobGroups.Data.Models.JobGroupModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobGroups.UnitTests.ControllerTests.SitemapControllerTests
{
    [Trait("Category", "Sitemap Controller Unit Tests")]
    public class SitemapControllerViewTests : BaseSitemapControllerTests
    {
        [Fact]
        public async Task SitemapControllerViewReturnsSuccess()
        {
            // Arrange
            const int resultsCount = 2;
            var expectedResults = A.CollectionOfFake<JobGroupModel>(resultsCount);
            var controller = BuildSitemapController();

            A.CallTo(() => FakeJobGroupDocumentService.GetAllAsync(A<string>.Ignored)).Returns(expectedResults);

            // Act
            var result = await controller.SitemapView().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupDocumentService.GetAllAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var contentResult = Assert.IsType<ContentResult>(result);

            contentResult.ContentType.Should().Be(MediaTypeNames.Application.Xml);

            controller.Dispose();
        }

        [Fact]
        public async Task SitemapControllerViewReturnsSuccessWhenNoData()
        {
            // Arrange
            const int resultsCount = 0;
            var expectedResults = A.CollectionOfFake<JobGroupModel>(resultsCount);
            var controller = BuildSitemapController();

            A.CallTo(() => FakeJobGroupDocumentService.GetAllAsync(A<string>.Ignored)).Returns(expectedResults);

            // Act
            var result = await controller.SitemapView().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupDocumentService.GetAllAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();

            _ = Assert.IsType<NoContentResult>(result);

            controller.Dispose();
        }
    }
}

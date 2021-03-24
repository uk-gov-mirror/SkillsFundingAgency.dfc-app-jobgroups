using DFC.App.JobGroups.Data.Models.JobGroupModels;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobGroups.UnitTests.ControllerTests.ApiControllerTests
{
    [Trait("Category", "API Controller Unit Tests")]
    public class ApiControllerTests : BaseApiControllerTests
    {
        [Fact]
        public async Task ApiControllerGetSummaryReturnsSuccess()
        {
            // Arrange
            var getSummaryResponse = new List<JobGroupModel>
            {
                 new JobGroupModel
                 {
                     Id = Guid.NewGuid(),
                     Soc = 1,
                     Title = "A title 1",
                 },
                 new JobGroupModel
                 {
                     Id = Guid.NewGuid(),
                     Soc = 2,
                     Title = "A title 2",
                 },
            };
            var controller = BuildApiController();

            A.CallTo(() => FakeJobGroupDocumentService.GetAllAsync(A<string>.Ignored)).Returns(getSummaryResponse);

            // Act
            var results = await controller.Get().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupDocumentService.GetAllAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(getSummaryResponse.Count, results!.Count());
        }

        [Fact]
        public async Task ApiControllerGetSummaryReturnsNullWhenNoData()
        {
            // Arrange
            List<JobGroupModel>? nullSummaryResponse = null;
            var controller = BuildApiController();

            A.CallTo(() => FakeJobGroupDocumentService.GetAllAsync(A<string>.Ignored)).Returns(nullSummaryResponse);

            // Act
            var results = await controller.Get().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupDocumentService.GetAllAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Null(results);
        }

        [Fact]
        public async Task ApiControllerGetDetailByIdReturnsSuccess()
        {
            // Arrange
            var getDetailResponse = new JobGroupModel
            {
                Id = Guid.NewGuid(),
                Soc = 1,
                Title = "A title 1",
            };
            var controller = BuildApiController();

            A.CallTo(() => FakeJobGroupDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(getDetailResponse);

            // Act
            var result = await controller.Get(Guid.NewGuid()).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(getDetailResponse, result);
        }

        [Fact]
        public async Task ApiControllerGetDetailBySocReturnsSuccess()
        {
            // Arrange
            var getDetailResponse = new JobGroupModel
            {
                Id = Guid.NewGuid(),
                Soc = 1,
                Title = "A title 1",
            };
            var controller = BuildApiController();

            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).Returns(getDetailResponse);

            // Act
            var result = await controller.Get(3543).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(getDetailResponse, result);
        }
    }
}

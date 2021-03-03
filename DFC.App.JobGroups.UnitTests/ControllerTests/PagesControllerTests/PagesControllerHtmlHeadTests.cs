using DFC.App.JobGroups.Data.Models.JobGroupModels;
using DFC.App.JobGroups.Models;
using DFC.App.JobGroups.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobGroups.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller - HtmlHead Unit Tests")]
    public class PagesControllerHtmlHeadTests : BasePagesControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerHtmlHeadHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var dummyJobGroupModel = A.Dummy<JobGroupModel>();
            var controller = BuildPagesController(mediaTypeName);
            var socRequestModel = new SocRequestModel { Soc = 3231, FromJobProfileCanonicalName = "a-job-profile", };
            var dummyHtmlHeadViewModel = A.Dummy<HtmlHeadViewModel>();

            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).Returns(dummyJobGroupModel);
            A.CallTo(() => FakeMapper.Map<HtmlHeadViewModel>(A<JobGroupModel>.Ignored)).Returns(dummyHtmlHeadViewModel);

            // Act
            var result = await controller.HtmlHead(socRequestModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<HtmlHeadViewModel>(A<JobGroupModel>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<HtmlHeadViewModel>(viewResult.ViewData.Model);
            var model = viewResult.ViewData.Model as HtmlHeadViewModel;
            Assert.Equal(dummyHtmlHeadViewModel, model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerHtmlHeadJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var dummyJobGroupModel = A.Dummy<JobGroupModel>();
            var controller = BuildPagesController(mediaTypeName);
            var socRequestModel = new SocRequestModel { Soc = 3231, FromJobProfileCanonicalName = "a-job-profile", };
            var dummyHtmlHeadViewModel = A.Dummy<HtmlHeadViewModel>();

            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).Returns(dummyJobGroupModel);
            A.CallTo(() => FakeMapper.Map<HtmlHeadViewModel>(A<JobGroupModel>.Ignored)).Returns(dummyHtmlHeadViewModel);

            // Act
            var result = await controller.HtmlHead(socRequestModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<HtmlHeadViewModel>(A<JobGroupModel>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            _ = Assert.IsAssignableFrom<HtmlHeadViewModel>(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerHtmlHeadReturnsNoContentWhenNoData(string mediaTypeName)
        {
            // Arrange
            JobGroupModel? nullJobGroupModel = null;
            var controller = BuildPagesController(mediaTypeName);
            var socRequestModel = new SocRequestModel { Soc = 3231, FromJobProfileCanonicalName = "a-job-profile", };

            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).Returns(nullJobGroupModel);

            // Act
            var result = await controller.HtmlHead(socRequestModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task PagesControllerHtmlHeadReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var dummyJobGroupModel = A.Dummy<JobGroupModel>();
            var controller = BuildPagesController(mediaTypeName);
            var socRequestModel = new SocRequestModel { Soc = 3231, FromJobProfileCanonicalName = "a-job-profile", };
            var dummyHtmlHeadViewModel = A.Dummy<HtmlHeadViewModel>();

            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).Returns(dummyJobGroupModel);
            A.CallTo(() => FakeMapper.Map<HtmlHeadViewModel>(A<JobGroupModel>.Ignored)).Returns(dummyHtmlHeadViewModel);

            // Act
            var result = await controller.HtmlHead(socRequestModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<HtmlHeadViewModel>(A<JobGroupModel>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}

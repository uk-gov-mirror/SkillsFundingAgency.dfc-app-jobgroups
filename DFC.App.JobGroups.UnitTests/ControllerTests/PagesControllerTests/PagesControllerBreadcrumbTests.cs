using DFC.App.JobGroups.Controllers;
using DFC.App.JobGroups.Data.Models.JobGroupModels;
using DFC.App.JobGroups.Models;
using DFC.App.JobGroups.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobGroups.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller - Breadcrumb Unit Tests")]
    public class PagesControllerBreadcrumbTests : BasePagesControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerBreadcrumbHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var socRequestModel = new SocRequestModel { Soc = 3231, FromJobProfileCanonicalName = "a-job-profile", };
            var dummyJobGroupModel = A.Dummy<JobGroupModel>();
            dummyJobGroupModel.JobProfiles = new List<JobProfileModel> { new JobProfileModel { CanonicalName = socRequestModel.FromJobProfileCanonicalName, Title = "A title" }, };
            var controller = BuildPagesController(mediaTypeName);
            var breadcrumbViewModel = new BreadcrumbViewModel
            {
                Breadcrumbs = new List<BreadcrumbItemViewModel>
                 {
                    new BreadcrumbItemViewModel
                    {
                        Title = "Home: Explore careers",
                        Route = "/",
                    },
                    new BreadcrumbItemViewModel
                    {
                        Title = dummyJobGroupModel.JobProfiles.First().Title,
                        Route = $"/job-profiles/" + dummyJobGroupModel.JobProfiles.First().CanonicalName,
                    },
                    new BreadcrumbItemViewModel
                    {
                        Title = "Job group LMI",
                        Route = $"/{PagesController.RegistrationPath}",
                        AddHyperlink = false,
                    },
                 },
            };
            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).Returns(dummyJobGroupModel);

            // Act
            var result = await controller.Breadcrumb(socRequestModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<BreadcrumbViewModel>(viewResult.ViewData.Model);
            var viewModel = viewResult.ViewData.Model as BreadcrumbViewModel;
            Assert.Equal(breadcrumbViewModel.Breadcrumbs[1].Title, viewModel?.Breadcrumbs?[1].Title);
            Assert.Equal(breadcrumbViewModel.Breadcrumbs[1].Route, viewModel?.Breadcrumbs?[1].Route);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerBreadcrumbJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var socRequestModel = new SocRequestModel { Soc = 3231, FromJobProfileCanonicalName = "a-job-profile", };
            var dummyJobGroupModel = A.Dummy<JobGroupModel>();
            dummyJobGroupModel.JobProfiles = new List<JobProfileModel> { new JobProfileModel { CanonicalName = socRequestModel.FromJobProfileCanonicalName, Title = "A title" }, };
            var controller = BuildPagesController(mediaTypeName);
            var breadcrumbViewModel = new BreadcrumbViewModel
            {
                Breadcrumbs = new List<BreadcrumbItemViewModel>
                 {
                    new BreadcrumbItemViewModel
                    {
                        Title = "Home: Explore careers",
                        Route = "/",
                    },
                    new BreadcrumbItemViewModel
                    {
                        Title = dummyJobGroupModel.JobProfiles.First().Title,
                        Route = $"/job-profiles/" + dummyJobGroupModel.JobProfiles.First().CanonicalName,
                    },
                    new BreadcrumbItemViewModel
                    {
                        Title = "Job group LMI",
                        Route = $"/{PagesController.RegistrationPath}",
                        AddHyperlink = false,
                    },
                 },
            };

            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).Returns(dummyJobGroupModel);

            // Act
            var result = await controller.Breadcrumb(socRequestModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<BreadcrumbViewModel>(jsonResult.Value);
            var viewModel = jsonResult.Value as BreadcrumbViewModel;
            Assert.Equal(breadcrumbViewModel.Breadcrumbs[1].Title, viewModel?.Breadcrumbs?[1].Title);
            Assert.Equal(breadcrumbViewModel.Breadcrumbs[1].Route, viewModel?.Breadcrumbs?[1].Route);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerBreadcrumbReturnsNoContentWhenNoData(string mediaTypeName)
        {
            // Arrange
            JobGroupModel? nullJobGroupModel = null;
            var controller = BuildPagesController(mediaTypeName);
            var socRequestModel = new SocRequestModel { Soc = 3231, FromJobProfileCanonicalName = "a-job-profile", };

            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).Returns(nullJobGroupModel);

            // Act
            var result = await controller.Breadcrumb(socRequestModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task PagesControllerBreadcrumbReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var dummyJobGroupModel = A.Dummy<JobGroupModel>();
            var controller = BuildPagesController(mediaTypeName);
            var socRequestModel = new SocRequestModel { Soc = 3231, FromJobProfileCanonicalName = "a-job-profile", };

            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).Returns(dummyJobGroupModel);

            // Act
            var result = await controller.Breadcrumb(socRequestModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobGroupDocumentService.GetAsync(A<Expression<Func<JobGroupModel, bool>>>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}

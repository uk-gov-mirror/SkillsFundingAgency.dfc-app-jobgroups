using DFC.App.JobGroups.Controllers;
using DFC.App.JobGroups.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace DFC.App.JobGroups.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller - Index Breadcrumb Unit Tests")]
    public class PagesControllerIndexBreadcrumbTests : BasePagesControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void PagesControllerIndexBreadcrumbHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
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
                        Title = "Job group LMI",
                        Route = $"/{PagesController.RegistrationPath}",
                        AddHyperlink = false,
                    },
                 },
            };

            // Act
            var result = controller.IndexBreadcrumb();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<BreadcrumbViewModel>(viewResult.ViewData.Model);
            var viewModel = viewResult.ViewData.Model as BreadcrumbViewModel;
            Assert.Equal(breadcrumbViewModel.Breadcrumbs[1].Title, viewModel?.Breadcrumbs?[1].Title);
            Assert.Equal(breadcrumbViewModel.Breadcrumbs[1].Route, viewModel?.Breadcrumbs?[1].Route);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void PagesControllerIndexBreadcrumbJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
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
                        Title = "Job group LMI",
                        Route = $"/{PagesController.RegistrationPath}",
                        AddHyperlink = false,
                    },
                 },
            };

            // Act
            var result = controller.IndexBreadcrumb();

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<BreadcrumbViewModel>(jsonResult.Value);
            var viewModel = jsonResult.Value as BreadcrumbViewModel;
            Assert.Equal(breadcrumbViewModel.Breadcrumbs[1].Title, viewModel?.Breadcrumbs?[1].Title);
            Assert.Equal(breadcrumbViewModel.Breadcrumbs[1].Route, viewModel?.Breadcrumbs?[1].Route);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void PagesControllerIndexBreadcrumbReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.IndexBreadcrumb();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}

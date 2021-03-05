using DFC.App.JobGroups.Data.Models.ContentModels;
using DFC.App.JobGroups.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobGroups.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller - IndexSideBarRight Unit Tests")]
    public class PagesControllerIndexSideBarRightTests : BasePagesControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerIndexSideBarRightHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var dummyContentItemModel = A.Dummy<ContentItemModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeSharedContentDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(dummyContentItemModel);

            // Act
            var result = await controller.IndexSideBarRight().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeSharedContentDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<SideBarRightViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerIndexSideBarRightJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var dummyContentItemModel = A.Dummy<ContentItemModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeSharedContentDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(dummyContentItemModel);

            // Act
            var result = await controller.IndexSideBarRight().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeSharedContentDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            _ = Assert.IsAssignableFrom<SideBarRightViewModel>(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task PagesControllerIndexSideBarRightReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var dummyContentItemModel = A.Dummy<ContentItemModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeSharedContentDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(dummyContentItemModel);

            // Act
            var result = await controller.IndexSideBarRight().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeSharedContentDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}

using DFC.App.JobGroups.Controllers;
using DFC.App.JobGroups.Data.Models.JobGroupModels;
using DFC.Compui.Cosmos.Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DFC.App.JobGroups.UnitTests.ControllerTests.SitemapControllerTests
{
    public class BaseSitemapControllerTests
    {
        public BaseSitemapControllerTests()
        {
            FakeLogger = A.Fake<ILogger<SitemapController>>();
            FakeJobGroupDocumentService = A.Fake<IDocumentService<JobGroupModel>>();
        }

        protected ILogger<SitemapController> FakeLogger { get; }

        protected IDocumentService<JobGroupModel> FakeJobGroupDocumentService { get; }

        protected SitemapController BuildSitemapController()
        {
            var controller = new SitemapController(FakeLogger, FakeJobGroupDocumentService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext(),
                },
            };

            return controller;
        }
    }
}

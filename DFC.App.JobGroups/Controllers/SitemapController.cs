using DFC.App.JobGroups.Data.Models.JobGroupModels;
using DFC.App.JobGroups.Extensions;
using DFC.App.JobGroups.Models;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DFC.App.JobGroups.Controllers
{
    public class SitemapController : Controller
    {
        public const string SitemapViewCanonicalName = "sitemap";

        private readonly ILogger<SitemapController> logger;
        private readonly IDocumentService<JobGroupModel> jobGroupDocumentService;

        public SitemapController(ILogger<SitemapController> logger, IDocumentService<JobGroupModel> jobGroupDocumentService)
        {
            this.logger = logger;
            this.jobGroupDocumentService = jobGroupDocumentService;
        }

        [HttpGet]
        [Route("pages/sitemap")]
        public async Task<IActionResult> SitemapView()
        {
            var result = await Sitemap().ConfigureAwait(false);

            return result;
        }

        [HttpGet]
        [Route("/sitemap.xml")]
        public async Task<IActionResult> Sitemap()
        {
            logger.LogInformation("Generating Sitemap");

            var sitemapUrlPrefix = $"{Request.GetBaseAddress()}";
            var sitemap = new Sitemap();
            var contentPageModels = await jobGroupDocumentService.GetAllAsync().ConfigureAwait(false);

            if (contentPageModels != null)
            {
                var contentPageModelsList = contentPageModels.ToList();

                if (contentPageModelsList.Any())
                {
                    var sitemapContentPageModels = contentPageModelsList
                         .OrderBy(o => o.Soc);

                    foreach (var contentPageModel in sitemapContentPageModels)
                    {
                        sitemap.Add(new SitemapLocation
                        {
                            Url = $"{sitemapUrlPrefix}{PagesController.RegistrationPath}/{contentPageModel.Soc}",
                            ChangeFrequency = SitemapLocation.ChangeFrequencies.Yearly,
                        });
                    }
                }
            }

            if (!sitemap.Locations.Any())
            {
                return NoContent();
            }

            var xmlString = sitemap.WriteSitemapToString();

            logger.LogInformation("Generated Sitemap");

            return Content(xmlString, MediaTypeNames.Application.Xml);
        }
    }
}
using DFC.App.JobGroups.Data.Contracts;
using DFC.App.JobGroups.Data.Models.JobGroupModels;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.JobGroups.Services.CacheContentService
{
    public class JobGroupCacheRefreshService : IJobGroupCacheRefreshService
    {
        private readonly ILogger<JobGroupCacheRefreshService> logger;
        private readonly IDocumentService<JobGroupModel> jobGroupDocumentService;
        private readonly ILmiTransformationApiConnector lmiTransformationApiConnector;

        public JobGroupCacheRefreshService(
            ILogger<JobGroupCacheRefreshService> logger,
            IDocumentService<JobGroupModel> jobGroupDocumentService,
            ILmiTransformationApiConnector lmiTransformationApiConnector)
        {
            this.logger = logger;
            this.jobGroupDocumentService = jobGroupDocumentService;
            this.lmiTransformationApiConnector = lmiTransformationApiConnector;
        }

        public async Task<HttpStatusCode> ReloadAsync(Uri url)
        {
            logger.LogInformation($"Refreshing all Job Groups from {url}");
            var summaries = await lmiTransformationApiConnector.GetSummaryAsync(url).ConfigureAwait(false);

            if (summaries != null && summaries.Any())
            {
                await PurgeAsync().ConfigureAwait(false);

                foreach (var item in summaries)
                {
                    await ReloadItemAsync(new Uri($"{url}/{item.Soc}", UriKind.Absolute)).ConfigureAwait(false);
                }
            }

            logger.LogInformation($"Refreshed all Job Groups from {url}");

            return HttpStatusCode.OK;
        }

        public async Task<HttpStatusCode> ReloadItemAsync(Uri url)
        {
            logger.LogInformation($"Getting Job Group item: {url}");

            var jobGroupModel = await lmiTransformationApiConnector.GetDetailsAsync(url).ConfigureAwait(false);

            if (jobGroupModel != null)
            {
                var existingJobGroup = await jobGroupDocumentService.GetAsync(w => w.Soc == jobGroupModel.Soc, jobGroupModel.Soc.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                if (existingJobGroup != null)
                {
                    jobGroupModel.Etag = existingJobGroup.Etag;
                }

                logger.LogInformation($"Upserting Job Groups item: {jobGroupModel.Soc} / {url}");
                return await jobGroupDocumentService.UpsertAsync(jobGroupModel).ConfigureAwait(false);
            }

            return HttpStatusCode.BadRequest;
        }

        public async Task<bool> PurgeAsync()
        {
            logger.LogInformation("Purging all Job Groups from");
            return await jobGroupDocumentService.PurgeAsync().ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(Guid contentId)
        {
            logger.LogInformation($"Deleting Job Groups item: {contentId}");
            return await jobGroupDocumentService.DeleteAsync(contentId).ConfigureAwait(false);
        }
    }
}

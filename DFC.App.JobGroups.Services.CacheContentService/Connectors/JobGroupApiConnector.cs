using DFC.App.JobGroups.Data.Contracts;
using DFC.App.JobGroups.Data.Models;
using DFC.App.JobGroups.Data.Models.JobGroupModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.JobGroups.Services.CacheContentService.Connectors
{
    public class JobGroupApiConnector : IJobGroupApiConnector
    {
        private readonly ILogger<JobGroupApiConnector> logger;
        private readonly HttpClient httpClient;
        private readonly IApiDataConnector apiDataConnector;

        public JobGroupApiConnector(
            ILogger<JobGroupApiConnector> logger,
            HttpClient httpClient,
            IApiDataConnector apiDataConnector)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            this.apiDataConnector = apiDataConnector;
        }

        public async Task<IList<JobGroupSummaryItemModel>?> GetSummaryAsync(Uri url)
        {
            logger.LogInformation($"Retrieving summaries from job groups API: {url}");
            return await apiDataConnector.GetAsync<IList<JobGroupSummaryItemModel>>(httpClient, url).ConfigureAwait(false);
        }

        public async Task<JobGroupModel?> GetDetailsAsync(Uri url)
        {
            logger.LogInformation($"Retrieving details from job groups API: {url}");
            return await apiDataConnector.GetAsync<JobGroupModel>(httpClient, url).ConfigureAwait(false);
        }
    }
}

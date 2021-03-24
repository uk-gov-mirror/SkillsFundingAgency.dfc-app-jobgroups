using AutoMapper;
using DFC.App.JobGroups.Data.Models;
using DFC.App.JobGroups.Data.Models.JobGroupModels;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.Jobgroups.Controllers
{
    [Route("api/job-groups")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> logger;
        private readonly IMapper mapper;
        private readonly IDocumentService<JobGroupModel> documentService;

        public ApiController(ILogger<ApiController> logger, IMapper mapper, IDocumentService<JobGroupModel> documentService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.documentService = documentService;
        }

        [HttpGet("summary")]
        public async Task<IEnumerable<JobGroupSummaryItemModel>?> Get()
        {
            var jobGroups = await documentService.GetAllAsync().ConfigureAwait(false);

            if (jobGroups == null)
            {
                return default;
            }

            var results = from a in jobGroups select mapper.Map<JobGroupSummaryItemModel>(a);

            return results;
        }

        [HttpGet("detail/{socId}")]
        public async Task<JobGroupModel?> Get(Guid socId)
        {
            return await documentService.GetByIdAsync(socId).ConfigureAwait(false);
        }

        [HttpGet("detail/soc/{soc}")]
        public async Task<JobGroupModel?> Get(int soc)
        {
            return await documentService.GetAsync(w => w.Soc == soc, soc.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
        }
    }
}

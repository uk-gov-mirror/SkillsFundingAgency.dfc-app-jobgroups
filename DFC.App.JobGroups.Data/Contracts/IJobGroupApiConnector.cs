using DFC.App.JobGroups.Data.Models;
using DFC.App.JobGroups.Data.Models.JobGroupModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.JobGroups.Data.Contracts
{
    public interface IJobGroupApiConnector
    {
        Task<IList<JobGroupSummaryItemModel>?> GetSummaryAsync(Uri url);

        Task<JobGroupModel?> GetDetailsAsync(Uri url);
    }
}

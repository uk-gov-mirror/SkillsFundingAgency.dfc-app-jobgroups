using DFC.App.JobGroups.Data.Models;
using DFC.App.JobGroups.Data.Models.JobGroupModels;
using DFC.App.JobGroups.Data.Models.LmiTransformationApiModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.JobGroups.Data.Contracts
{
    public interface ILmiTransformationApiConnector
    {
        Task<IList<JobGroupSummaryItemModel>?> GetSummaryAsync(Uri url);

        Task<JobGroupModel?> GetDetailsAsync(Uri url);
    }
}

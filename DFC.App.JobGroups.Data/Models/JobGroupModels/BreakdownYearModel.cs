using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobGroups.Data.Models.JobGroupModels
{
    [ExcludeFromCodeCoverage]
    public class BreakdownYearModel
    {
        public int Year { get; set; }

        public IList<BreakdownYearValueModel>? Breakdown { get; set; }
    }
}

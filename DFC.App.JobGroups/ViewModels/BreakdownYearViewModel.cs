using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobGroups.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class BreakdownYearViewModel
    {
        public int Year { get; set; }

        public IList<BreakdownYearValueViewModel>? Breakdown { get; set; }
    }
}

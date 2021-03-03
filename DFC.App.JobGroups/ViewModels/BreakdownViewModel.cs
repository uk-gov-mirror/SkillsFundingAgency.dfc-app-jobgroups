using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobGroups.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class BreakdownViewModel
    {
        public string? Note { get; set; }

        public BreakdownYearViewModel? PredictedEmployment { get; set; }
    }
}

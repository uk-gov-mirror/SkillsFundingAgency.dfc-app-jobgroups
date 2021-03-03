using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobGroups.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class BodyViewModel
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public JobGrowthViewModel? JobGrowth { get; set; }

        public QualificationLevelViewModel? QualificationLevel { get; set; }

        public BreakdownViewModel? EmploymentByRegion { get; set; }

        public BreakdownViewModel? TopIndustriesInJobGroup { get; set; }
    }
}

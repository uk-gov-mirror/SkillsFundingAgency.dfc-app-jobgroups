using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobGroups.Data.Models.JobGroupModels
{
    [ExcludeFromCodeCoverage]
    public class JobGrowthPredictionModel
    {
        public int StartYearRange { get; set; }

        public int EndYearRange { get; set; }

        public decimal JobsCreated { get; set; }

        public decimal PercentageGrowth { get; set; }
    }
}

using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobGroups.Data.Models.LmiTransformationApiModels
{
    [ExcludeFromCodeCoverage]
    public class JobGroupSummaryItemModel
    {
        public int Soc { get; set; }

        public string? Title { get; set; }
    }
}

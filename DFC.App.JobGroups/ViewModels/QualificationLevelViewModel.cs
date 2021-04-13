using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobGroups.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class QualificationLevelViewModel
    {
        public int Year { get; set; }

        public int Code { get; set; }

        public string? Name { get; set; }

        public string? Note { get; set; }

        public decimal Employment { get; set; }

        public string? LevelNumberFromName { get; set; }
    }
}

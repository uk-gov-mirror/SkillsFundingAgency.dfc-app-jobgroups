using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobGroups.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class HealthItemViewModel
    {
        public string? Service { get; set; }

        public string? Message { get; set; }
    }
}

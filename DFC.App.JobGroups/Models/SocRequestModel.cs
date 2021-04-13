using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobGroups.Models
{
    [ExcludeFromCodeCoverage]
    public class SocRequestModel
    {
        public int Soc { get; set; }

        public string? FromJobProfileCanonicalName { get; set; }
    }
}

using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobGroups.Data.Models.ClientOptions
{
    [ExcludeFromCodeCoverage]
    public class JobGroupDraftApiClientOptions : ClientOptionsModel
    {
        public string SummaryEndpoint { get; set; } = "summary";

        public string DetailEndpoint { get; set; } = "detail";
    }
}

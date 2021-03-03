using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobGroups.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class IndexViewModel
    {
        public string? LocalPath { get; set; }

        public List<IndexDocumentViewModel>? Documents { get; set; }
    }
}

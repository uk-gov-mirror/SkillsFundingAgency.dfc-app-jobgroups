using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobGroups.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class BreadcrumbViewModel
    {
        public List<BreadcrumbItemViewModel>? Breadcrumbs { get; set; }
    }
}

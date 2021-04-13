using Microsoft.AspNetCore.Html;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobGroups.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class SharedContentViewModel
    {
        public HtmlString? Markup { get; set; } = new HtmlString("Unknown content");
    }
}

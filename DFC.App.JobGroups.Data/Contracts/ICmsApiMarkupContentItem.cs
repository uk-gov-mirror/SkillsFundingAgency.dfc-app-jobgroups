namespace DFC.App.JobGroups.Data.Models.Contracts
{
    public interface ICmsApiMarkupContentItem
    {
        string? Title { get; set; }

        public string? Content { get; set; }

        public string? HtmlBody { get; set; }
    }
}
using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobGroups.Data.Models.ClientOptions
{
    [ExcludeFromCodeCoverage]
    public class EventGridClientOptions
    {
        public string? TopicEndpoint { get; set; }

        public string? SubjectPrefix { get; set; }

        public string? TopicKey { get; set; }

        public Uri? ApiEndpoint { get; set; }
    }
}

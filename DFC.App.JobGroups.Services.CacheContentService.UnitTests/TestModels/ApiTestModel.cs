using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobGroups.Services.CacheContentService.UnitTests.TestModels
{
    [ExcludeFromCodeCoverage]
    public class ApiTestModel
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }
    }
}
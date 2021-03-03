using System.Net.Http;

namespace DFC.App.JobGroups.Services.CacheContentService.UnitTests.FakeHttpHandlers
{
    public interface IFakeHttpRequestSender
    {
        HttpResponseMessage Send(HttpRequestMessage request);
    }
}

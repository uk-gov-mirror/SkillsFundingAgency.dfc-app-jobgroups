using DFC.App.JobGroups.Data.Models;
using DFC.App.JobGroups.Data.Models.ClientOptions;
using System.Threading.Tasks;

namespace DFC.App.JobGroups.Data.Contracts
{
    public interface IEventGridService
    {
        Task SendEventAsync(EventGridEventData? eventGridEventData, string? subject, string? eventType);

        bool IsValidEventGridClientOptions(EventGridClientOptions? eventGridClientOptions);
    }
}

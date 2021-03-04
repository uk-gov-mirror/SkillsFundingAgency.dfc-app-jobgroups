using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.JobGroups.Data.Contracts
{
    public interface ISharedContentCacheReloadService
    {
        Task ReloadAsync(CancellationToken stoppingToken);
    }
}

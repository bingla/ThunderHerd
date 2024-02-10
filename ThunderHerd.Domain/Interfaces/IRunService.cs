using ThunderHerd.Core.Models.Dtos;

namespace ThunderHerd.Domain.Interfaces
{
    public interface IRunService
    {
        Task RunTestAsync(Guid testId, CancellationToken cancellationToken);
        Task RunTestAsync(Test? test, CancellationToken cancellationToken);
    }
}

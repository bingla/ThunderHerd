using ThunderHerd.Core.Models.Dtos;

namespace ThunderHerd.Domain.Interfaces
{
    public interface IRunService
    {
        Task<RunResult> RunAsync(Guid runId, CancellationToken cancellationToken);
        Task<RunResult> RunAsync(Run run, CancellationToken cancellationToken);
    }
}

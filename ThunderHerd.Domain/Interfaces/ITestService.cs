using ThunderHerd.Core.Models.Dtos;

namespace ThunderHerd.Domain.Interfaces
{
    public interface ITestService
    {
        Task<Test?> GetTestAsync(Guid testId, CancellationToken cancellationToken = default);
        Task<Test> CreateTestAsync(Test test, CancellationToken cancellationToken = default);
    }
}
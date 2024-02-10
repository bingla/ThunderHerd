using ThunderHerd.Core.Models.Dtos;

namespace ThunderHerd.Domain.Interfaces
{
    public interface ITestResultService
    {
        Task<TestResult?> CreateTestResultAsync(TestResult testResult, CancellationToken cancellationToken = default);
        Task<TestResult?> FindTestResultAsync(Guid testResultId, CancellationToken cancellationToken = default);
        IAsyncEnumerable<TestResult?> FindTestResultsByTestId(Guid testId);
        IAsyncEnumerable<TestResultItem?> FindTestResultItemsByTestResultId(Guid testResultId);
    }
}

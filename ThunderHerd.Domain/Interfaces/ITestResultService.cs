using ThunderHerd.Core.Models.Dtos;

namespace ThunderHerd.Domain.Interfaces
{
    public interface ITestResultService
    {
        Task<TestResult?> CreateTestResultAsync(TestResult testResult, CancellationToken cancellationToken = default);
        Task<TestResult?> UpdateTestResultAsync(TestResult testResult, CancellationToken cancellationToken = default);
        Task<TestResult?> FindTestResultAsync(Guid testResultId, CancellationToken cancellationToken = default);

        IAsyncEnumerable<TestResult?> FindTestResultsByTestId(Guid testId);
        Task<IEnumerable<TestResultGroup>> GroupTestResultItemsByTime(Guid testResultId, TimeSpan timespan);

        Task CreateTestResultItem(TestResultItem testResultItem, CancellationToken cancellationToken);
        Task CreateTestResultItems(IEnumerable<TestResultItem> testResultItems, CancellationToken cancellationToken);
    }
}

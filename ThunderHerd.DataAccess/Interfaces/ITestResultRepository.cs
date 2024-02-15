using ThunderHerd.Core.Entities;

namespace ThunderHerd.DataAccess.Interfaces
{
    public interface ITestResultRepository : IGeneralRepository<TestResult>
    {
        IAsyncEnumerable<TestResult> GetTestResultsByTestId(Guid testId);
    }
}

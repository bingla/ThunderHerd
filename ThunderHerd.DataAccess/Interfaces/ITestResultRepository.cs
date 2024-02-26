using ThunderHerd.Core.Entities;
using ThunderHerd.Core.Enums;

namespace ThunderHerd.DataAccess.Interfaces
{
    public interface ITestResultRepository : IGeneralRepository<TestResult>
    {
        IAsyncEnumerable<TestResult> GetTestResultsByTestId(Guid testId);
    }
}

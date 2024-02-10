using ThunderHerd.Core.Entities;

namespace ThunderHerd.DataAccess.Interfaces
{
    public interface ITestResultItemRepository : IGeneralRepository<TestResultItem>
    {
        IAsyncEnumerable<TestResultItem> GetTestResultItems(Guid testResultId);
    }
}

using ThunderHerd.Core.Entities;

namespace ThunderHerd.DataAccess.Interfaces
{
    public interface ITestResultItemRepository : IGeneralRepository<TestResultItem>
    {
        Task<IEnumerable<TestResultItem>> GetTestResultItemsAsync(Guid testResultId);
    }
}

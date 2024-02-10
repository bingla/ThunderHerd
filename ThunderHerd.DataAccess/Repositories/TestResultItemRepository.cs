using Microsoft.EntityFrameworkCore;
using ThunderHerd.Core.Entities;
using ThunderHerd.DataAccess.Interfaces;

namespace ThunderHerd.DataAccess.Repositories
{
    public class TestResultItemRepository : GeneralRepository<TestResultItem>, ITestResultItemRepository
    {
        public TestResultItemRepository(ThunderHerdContext context) : base(context)
        { }

        public IAsyncEnumerable<TestResultItem> GetTestResultItems(Guid testResultId)
        {
            return _set
                .Where(p => p.TestResultId == testResultId)
                .AsAsyncEnumerable();
        }
    }
}

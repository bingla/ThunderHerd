using Microsoft.EntityFrameworkCore;
using ThunderHerd.Core.Entities;
using ThunderHerd.DataAccess.Interfaces;

namespace ThunderHerd.DataAccess.Repositories
{
    public class TestResultItemRepository : GeneralRepository<TestResultItem>, ITestResultItemRepository
    {
        public TestResultItemRepository(ThunderHerdContext context) : base(context)
        { }

        public async Task<IEnumerable<TestResultItem>> GetTestResultItemsAsync(Guid testResultId)
        {
            return await _set.Where(p => p.TestResultId == testResultId).ToListAsync();
        }
    }
}

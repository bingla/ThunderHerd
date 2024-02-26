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
            return await GetTestResultItemsAsync(testResultId, false);
        }

        public async Task<IEnumerable<TestResultItem>> GetTestResultItemsAsync(Guid testResultId, bool asNoTracking)
        {
            var query = _context.TestResultItem.Where(p => p.TestResultId == testResultId);

            return asNoTracking
                ? await query.AsNoTracking().ToListAsync()
                : await query.ToListAsync();
        }
    }
}

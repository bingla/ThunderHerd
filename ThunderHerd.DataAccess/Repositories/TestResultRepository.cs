using Microsoft.EntityFrameworkCore;
using ThunderHerd.Core.Entities;
using ThunderHerd.DataAccess.Interfaces;

namespace ThunderHerd.DataAccess.Repositories
{
    public class TestResultRepository : GeneralRepository<TestResult>, ITestResultRepository
    {
        public TestResultRepository(ThunderHerdContext context) : base(context)
        { }

        public IAsyncEnumerable<TestResult> GetTestResultsByTestId(Guid testId)
        {
            return _set
                .Where(p => p.TestId == testId)
                .AsAsyncEnumerable();
        }
    }
}

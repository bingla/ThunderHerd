using ThunderHerd.Core.Entities;
using ThunderHerd.DataAccess.Interfaces;

namespace ThunderHerd.DataAccess.Repositories
{
    public class TestRepository : GeneralRepository<Test>, ITestRepository
    {
        public TestRepository(ThunderHerdContext context) : base(context)
        { }
    }
}

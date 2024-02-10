using ThunderHerd.Core.Entities;
using ThunderHerd.DataAccess.Interfaces;

namespace ThunderHerd.DataAccess.Repositories
{
    public class RunRepository : GeneralRepository<Run>, IRunRepository
    {
        public RunRepository(ThunderHerdContext context) : base(context)
        { }
    }
}

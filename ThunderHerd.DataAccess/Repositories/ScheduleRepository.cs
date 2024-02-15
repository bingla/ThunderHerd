using ThunderHerd.Core.Entities;
using ThunderHerd.DataAccess.Interfaces;

namespace ThunderHerd.DataAccess.Repositories
{
    public class ScheduleRepository : GeneralRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(ThunderHerdContext context) : base(context)
        { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderHerd.Domain.Interfaces;

namespace ThunderHerd.Domain.Services
{
    /// <summary>
    /// Run scheduler
    /// </summary>
    public class ScheduleService : IScheduleService
    {
        public ScheduleService()
        { }

        /// <summary>
        /// Schedule a run
        /// </summary>
        /// <remarks>
        /// Schedules a run by persisting it to DB and registering it in hangfire(?)
        /// </remarks>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task ScheduleRun()
        {
            throw new NotImplementedException();
        }
    }
}

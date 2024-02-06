using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThunderHerd.Domain.Interfaces
{
    public interface IScheduleService
    {
        Task ScheduleRun();
    }
}

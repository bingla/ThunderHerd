using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderHerd.Core.Models.Dtos;

namespace ThunderHerd.Domain.Interfaces
{
    public interface IScheduleService
    {
        Task ScheduleRunAsync(Run run, CancellationToken cancellationToken);
    }
}

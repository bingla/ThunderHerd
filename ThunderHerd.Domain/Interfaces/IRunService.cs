using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderHerd.Core.Models.Dtos;

namespace ThunderHerd.Domain.Interfaces
{
    public interface IRunService
    {
        Task<RunResult> RunAsync(Run run, CancellationToken cancellationToken);
    }
}

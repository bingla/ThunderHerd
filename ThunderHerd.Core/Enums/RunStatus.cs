using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThunderHerd.Core.Enums
{
    public enum RunStatus
    {
        Starting = 0,
        Warmup = 1,
        Running = 2,
        Stopping = 3,
        Stopped = 4,
    }
}

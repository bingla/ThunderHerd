using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThunderHerd.Core.Options
{
    public class RunServiceOption
    {
        public int MaxParallelThreads { get; set; } = 4;
        public int MaxCallsPerSecond { get; set; } = 100;
        public int MaxRunDurationInMinutes { get; set; } = 60;
        public int MaxWarmupDurationInMinutes { get; set; } = 60;
    }
}

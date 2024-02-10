using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThunderHerd.Core.Entities
{
    public class Run
    {
        public Guid Id { get; set; }

        public string? AppId { get; set; }
        public string? AppSecret { get; set; }
        public string? ApiKey { get; set; }

        public int CallsPerSecond { get; set; } = 1;
        public int RunDurationInMinutes { get; set; } = 1;
        public int WarmupDurationInMinutes { get; set; }

        public bool Recurring { get; set; } = false;
        public string? CronSchedule { get; set; }

        public IEnumerable<TestItem> TestCollection { get; init; } = new HashSet<TestItem>();

        public static Run Map(Models.Dtos.Run entity)
        {
            return new Run
            {
                Id = entity.Id,
                AppId = entity.AppId,
                AppSecret = entity.AppSecret,
                ApiKey = entity.ApiKey,
                CallsPerSecond = entity.CallsPerSecond,
                RunDurationInMinutes = entity.RunDurationInMinutes,
                WarmupDurationInMinutes = entity.WarmupDurationInMinutes,
                Recurring = entity.Recurring,
                CronSchedule = entity.CronSchedule,
            };
        }

        public record RunId(Guid Id);
    }    
}

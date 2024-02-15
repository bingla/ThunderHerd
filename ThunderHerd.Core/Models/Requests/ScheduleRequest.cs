using System.ComponentModel.DataAnnotations;

namespace ThunderHerd.Core.Models.Requests
{
    public class ScheduleRequest
    {
        [Required]
        public bool Recurring { get; set; } = false;
        public string? CronSchedule { get; set; }
    }
}

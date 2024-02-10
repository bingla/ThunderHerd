using System.ComponentModel.DataAnnotations;
using ThunderHerd.Core.Enums;

namespace ThunderHerd.Core.Models.Requests
{
    public class RunRequest
    {
        [Required]
        public string? AppId { get; set; }
        [Required]
        public string? AppSecret { get; set; }
        public string? ApiKey { get; set; }

        public int CallsPerSecond { get; set; } = 1;
        public int RunDurationInMinutes { get; set; }
        public int WarmupDurationInMinutes { get; set; }

        public bool Recurring { get; set; } = false;
        public string? CronSchedule { get; set; } 

        [Required]
        public IEnumerable<TestItem> TestCollection { get; init; } = new HashSet<TestItem>();

        public class TestItem
        {
            [Required]
            public HttpMethods Method { get; set; } = HttpMethods.GET;
            [Required]
            public string? Url { get; set; }
        }
    }
}

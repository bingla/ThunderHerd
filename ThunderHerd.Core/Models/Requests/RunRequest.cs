using System.ComponentModel.DataAnnotations;
using ThunderHerd.Core.Enums;

namespace ThunderHerd.Core.Models.Requests
{
    public partial class RunRequest
    {
        [Required]
        public string? AppId { get; set; }
        [Required]
        public string? AppSecret { get; set; }
        public string? ApiKey { get; set; }

        public int CallsPerSecond { get; set; } = 1;
        public int RunDurationInMinutes { get; set; }
        public int WarmupDurationInMinutes { get; set; }

        [Required]
        public ICollection<TestItem> TestCollection { get; set; } = new HashSet<TestItem>();
    }

    public partial class RunRequest
    {
        public class TestItem
        {
            [Required]
            public HttpMethods Method { get; set; } = HttpMethods.GET;
            [Required]
            public string? Url { get; set; }
        }
    }
}

using System.ComponentModel.DataAnnotations;
using ThunderHerd.Core.Enums;

namespace ThunderHerd.Core.Models.Requests
{
    public class TestCreateRequest
    {
        public int CallsPerSecond { get; set; } = 1;
        public int RunDurationInMinutes { get; set; }
        public int WarmupDurationInMinutes { get; set; }

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

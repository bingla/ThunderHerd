using ThunderHerd.Core.Enums;
using ThunderHerd.Core.Models.Requests;

namespace ThunderHerd.Core.Models.Dtos
{
    public partial class Run
    {
        public string? AppId { get; set; }
        public string? AppSecret { get; set; }

        public int CallsPerSecond { get; set; } = 1;
        public int RunDurationInMinutes { get; set; } = 1;
        public int WarmupDurationInMinutes { get; set; }

        public IEnumerable<TestItem> TestCollection { get; init; } = new HashSet<TestItem>();

        public static Run Map(RunRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            return new Run
                {
                    AppId = request.AppId,
                    AppSecret = request.AppSecret,
                    CallsPerSecond = request.CallsPerSecond,
                    RunDurationInMinutes = request.RunDurationInMinutes,
                    WarmupDurationInMinutes = request.WarmupDurationInMinutes,
                    TestCollection = request.TestCollection.Count > 0
                        ? request.TestCollection.Select(TestItem.Map).ToHashSet()
                        : Enumerable.Empty<TestItem>(),
                };
        }
    }

    public partial class Run
    {
        public class TestItem
        {
            public HttpMethods Method { get; set; } = HttpMethods.GET;
            public string? Url { get; set; }

            public static TestItem Map(RunRequest.TestItem item)
            {
                ArgumentNullException.ThrowIfNull(item, nameof(item));

                return new TestItem
                {
                    Method = item.Method,
                    Url = item.Url,
                };
            }
        }
    }
}

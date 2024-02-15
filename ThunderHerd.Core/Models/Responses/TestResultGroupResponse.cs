using ThunderHerd.Core.Models.Dtos;

namespace ThunderHerd.Core.Models.Responses
{
    public class TestResultGroupResponse
    {
        public long Ticks { get; set; }

        public IEnumerable<TestResultGroupItemResponse> Items { get; init; } = new List<TestResultGroupItemResponse>();

        public static TestResultGroupResponse Map(TestResultGroup entity)
        {
            return new TestResultGroupResponse
            {
                Ticks = entity.Ticks,
                Items = entity.Items.Select(TestResultGroupItemResponse.Map),
            };
        }
    }
}

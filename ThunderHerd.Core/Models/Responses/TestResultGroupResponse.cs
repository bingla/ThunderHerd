using System.Runtime.CompilerServices;
using ThunderHerd.Core.Enums;
using ThunderHerd.Core.Models.Dtos;

namespace ThunderHerd.Core.Models.Responses
{
    public class TestResultGroupResponse
    {
        public long Ticks { get; set; }

        public IEnumerable<TestResultGroupItemResponse> Items { get; init; } = new List<TestResultGroupItemResponse>();

        public static Task<TestResultGroupResponse> Map(TestResultGroup entity)
        {
            return Task.FromResult(new TestResultGroupResponse
            {
                Ticks = entity.Ticks,
                Items = entity.Items.Select(TestResultGroupItemResponse.Map),
            });
        }
    }
}

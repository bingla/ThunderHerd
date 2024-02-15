using System.Data;

namespace ThunderHerd.Core.Models.Dtos
{
    public class TestResultGroup
    {
        public long Ticks { get; set; }

        public IEnumerable<TestResultGroupItem> Items { get; set; } = new List<TestResultGroupItem>();

        public static TestResultGroup Map(IGrouping<long, TestResultItem> group)
        {
            return new TestResultGroup
            {
                Ticks = group.Key,
                Items = group
                           .GroupBy(p => p.AbsoluteUrl, p => p)
                           .Select(TestResultGroupItem.Map),
            };
        }
    }
}

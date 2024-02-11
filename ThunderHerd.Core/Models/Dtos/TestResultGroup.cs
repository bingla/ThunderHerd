using System.Data;
using System.Linq;
using ThunderHerd.Core.Enums;

namespace ThunderHerd.Core.Models.Dtos
{
    public class TestResultGroup
    {
        public long Ticks { get; set; }

        //public IAsyncEnumerable<TestResultGroupItem> Items { get; init; } = AsyncEnumerable.Empty<TestResultGroupItem>();
        public IEnumerable<TestResultGroupItem> Items { get; set; } = new List<TestResultGroupItem>();

        public static async IAsyncEnumerable<TestResultGroup> Map(IAsyncEnumerable<IAsyncGrouping<long, TestResultItem>> groups)
        {
            await foreach (var group in groups)
            {
                yield return new TestResultGroup
                {
                    Ticks = group.Key,
                    Items = group
                                .GroupBy(p => p.AbsoluteUrl, p => p)
                                .SelectAwait(async p => await TestResultGroupItem.Map(p))
                                .ToEnumerable(),
                };
            }
        }

        public static async Task<TestResultGroup> Map(IAsyncGrouping<long, TestResultItem> group)
        {
            var items = group
                            .GroupBy(p => p.AbsoluteUrl, p => p)
                            .SelectAwait(async p => await TestResultGroupItem.Map(p));

            return new TestResultGroup
            {
                //Ticks = (await group.FirstOrDefaultAsync()).Ticks,
                Ticks = group.Key,
                Items = await group
                            .GroupBy(p => p.AbsoluteUrl, p => p)
                            .SelectAwait(async p => await TestResultGroupItem.Map(p))
                            .ToListAsync(), // TODO: Find out why there are no rows here
            };
        }
    }
}

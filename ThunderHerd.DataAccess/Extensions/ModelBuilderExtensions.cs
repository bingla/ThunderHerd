using Microsoft.EntityFrameworkCore;
using ThunderHerd.Core.Entities;
using static ThunderHerd.Core.Entities.Run;
using static ThunderHerd.Core.Entities.TestItem;

namespace ThunderHerd.DataAccess.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder BuildRun(this ModelBuilder builder)
        {
            builder
                .Entity<Run>()
                .HasKey(p => p.Id);
                //.Property(p => p.Id)
                //.HasConversion(
                //    value => value.Id,
                //    value => new RunId(value));

            builder
                .Entity<Run>()
                .HasMany(p => p.TestCollection)
                .WithOne(p => p.Run);

            return builder;
        }

        public static ModelBuilder BuildTestItem(this ModelBuilder builder)
        {
            builder
                .Entity<TestItem>()
                .HasKey(p => p.Id);
                //.Property(p => p.Id)
                //.HasConversion(
                //    value => value.Id,
                //    value => new TestItemId(value));

            return builder;
        }
    }
}

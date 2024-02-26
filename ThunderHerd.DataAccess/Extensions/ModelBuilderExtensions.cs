using Microsoft.EntityFrameworkCore;
using ThunderHerd.Core.Entities;

namespace ThunderHerd.DataAccess.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder BuildTest(this ModelBuilder builder)
        {
            builder
                .Entity<Test>()
                .HasKey(p => p.Id);

            builder
                .Entity<Test>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder
                .Entity<Test>()
                .HasMany(p => p.TestItems)
                .WithOne(p => p.Test);

            builder
                .Entity<Test>()
                .Navigation(p => p.TestItems)
                .AutoInclude();


            return builder;
        }

        public static ModelBuilder BuildTestItem(this ModelBuilder builder)
        {
            builder
                .Entity<TestItem>()
                .HasKey(p => p.Id);

            builder
                .Entity<TestItem>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            return builder;
        }

        public static ModelBuilder BuildSchedule(this ModelBuilder builder)
        {
            builder
                .Entity<Schedule>()
                .HasKey(p => p.Id);

            builder
                .Entity<Schedule>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Schedule>()
                .HasOne(p => p.Test);

            return builder;
        }

        public static ModelBuilder BuildTestResult(this ModelBuilder builder)
        {
            builder
                .Entity<TestResult>()
                .HasKey(p => p.Id);

            builder
                .Entity<TestResult>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            return builder;
        }
        public static ModelBuilder BuildTestResultItem(this ModelBuilder builder)
        {
            builder
                .Entity<TestResultItem>()
                .HasKey(p => p.Id);

            builder
                .Entity<TestResultItem>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            return builder;
        }
    }
}

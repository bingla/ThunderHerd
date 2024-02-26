using Microsoft.EntityFrameworkCore;
using ThunderHerd.Core.Entities;
using ThunderHerd.DataAccess.Extensions;

namespace ThunderHerd.DataAccess
{
    public class ThunderHerdContext : DbContext
    {
        public DbSet<Test> Test { get; set; }
        public DbSet<TestItem> TestItem { get; set; }
        public DbSet<TestResult> TestResult { get; set; }
        public DbSet<TestResultItem> TestResultItem { get; set; }

        public ThunderHerdContext(DbContextOptions<ThunderHerdContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .BuildTest()
                .BuildTestItem()
                .BuildSchedule()
                .BuildTestResult()
                .BuildTestResultItem();

        }
    }
}

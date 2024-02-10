using Microsoft.EntityFrameworkCore;
using ThunderHerd.Core.Entities;
using ThunderHerd.DataAccess.Extensions;

namespace ThunderHerd.DataAccess
{
    public class ThunderHerdContext : DbContext
    {
        public DbSet<Run> Run { get; set; }
        public DbSet<TestItem> TestItem { get; set; }

        public ThunderHerdContext(DbContextOptions<ThunderHerdContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .BuildRun()
                .BuildTestItem();

        }
    }
}

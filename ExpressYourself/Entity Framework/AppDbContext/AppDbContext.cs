using ExpressYourself.Entity_Framework.Types;
using Microsoft.EntityFrameworkCore;

namespace ExpressYourself.Entity_Framework.DBContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<Countries> Countries { get; set; }
        public DbSet<IPAddresses> IPAddresses { get; set; }
        public DbSet<SQLReport> SQLReport { get; set; }
        public DbSet<IPToUpdate> IPToUpdate { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SQLReport>().HasNoKey();
            modelBuilder.Entity<IPToUpdate>().HasNoKey();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Repository.DataContext.Configuration;

namespace NETCore.Basic.Repository.DataContext
{
    public class NetDbContext : DbContext
    {
        public NetDbContext()
        { }
        public NetDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Migrations only works with this, somehow
            //optionsBuilder.UseSqlServer("Data Source=HED-GAMING;Initial Catalog=NETCoreAPIDb;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserDataConfiguration());
            builder.ApplyConfiguration(new EventLogConfiguration());
        }
        public DbSet<User> Users { get; set; }
        public DbSet<EventLog> Logs { get; set; }
    }
}

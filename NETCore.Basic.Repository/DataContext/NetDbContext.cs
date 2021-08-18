using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Repository.DataContext.Configuration;
using NETCore.Basic.Util.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Repository.DataContext
{
    public class NetDbContext : DbContext
    {
        public NetDbContext()
        {}
        public NetDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableDetailedErrors(true);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserDataConfiguration());
        }
        public DbSet<User> Users { get; set; }
    }
}

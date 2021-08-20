using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Repository.DataContext.Configuration;
using NETCore.Basic.Util.Configuration;
using NETCore.Basic.Util.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Repository.DataContext
{
    public class NetDbContext : DbContext
    {
        public IAPIConfigurations _configurations { get; }
        public NetDbContext()
        {}
        public NetDbContext(DbContextOptions options, IAPIConfigurations configurations) : base(options)
        {
            _configurations = configurations;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableDetailedErrors(true);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserDataConfiguration());
            builder.ApplyConfiguration(new EventLogConfiguration(_configurations));
        }
        public DbSet<User> Users { get; set; }
        public DbSet<EventLog> Logs { get; set; }
    }
}

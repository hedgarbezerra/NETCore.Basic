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
    public class Context : DbContext
    {
        private IConfiguration _configuration { get; }
        public Context()
        {

        }
        public Context(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserDataConfiguration());
            builder.Entity<User>().ToTable("TbUsuarios");
        }
        public DbSet<User> Users { get; set; }
    }
}

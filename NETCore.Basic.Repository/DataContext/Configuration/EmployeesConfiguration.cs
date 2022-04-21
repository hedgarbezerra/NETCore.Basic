using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NETCore.Basic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Repository.DataContext.Configuration
{
    public sealed class EmployeesConfiguration : DataConfiguration<Employee>
    {
        public EmployeesConfiguration() : base()
        {
        }
        protected override void ConfigurateFields(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(c => c.Id)
               .HasColumnType("int")
               .HasColumnName("Id");

            builder.Property(c => c.Name)
               .HasColumnType("varchar(255)")
               .HasColumnName("Name")
               .IsRequired(true);

            builder.Property(c => c.Email)
               .HasColumnType("varchar(255)")
               .HasColumnName("Email")
               .IsRequired(true);

            builder.Property(c => c.RegisteredAt)
               .HasColumnType("datetime")
               .HasColumnName("Registered")
               .HasDefaultValueSql("GetDate()");

            builder.OwnsOne(a => a.Type, tipo =>
            {
                tipo.Property(a => a.Value)
                .HasColumnType("int")
                .HasColumnName("EmployeeType");

                tipo.Ignore(a => a.DisplayName);
            });

        }

        protected override void ConfigurateFK(EntityTypeBuilder<Employee> builder)
        {
        }

        protected override void ConfiguratePK(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(x => x.Id);
        }

        protected override void ConfigurateTableName(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("tbEmployees");
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NETCore.Basic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Repository.DataContext.Configuration
{
    public class UserDataConfiguration : DataConfiguration<User>
    {
        protected override void ConfigurateFields(EntityTypeBuilder<User> builder)
        {
            builder.Property(c => c.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
            builder.Property(c => c.Name)
                .HasMaxLength(255)
                .IsRequired();

        }

        protected override void ConfigurateFK(EntityTypeBuilder<User> builder)
        {
        }

        protected override void ConfiguratePK(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(c => c.Id);
        }

    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NETCore.Basic.Domain.Entities;
using System;

namespace NETCore.Basic.Repository.DataContext.Configuration
{
    public sealed class UserDataConfiguration : DataConfiguration<User>
    {
        protected override void ConfigurateFields(EntityTypeBuilder<User> builder)
        {
            builder.Property(c => c.Id)
                .HasColumnType("int")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(c => c.Password)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(c => c.Username)
                .HasColumnType("varchar")
                .HasMaxLength(25)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(c => c.RegistredAt)
                .HasColumnType("datetime")
                .IsRequired();

        }

        protected override void ConfigurateFK(EntityTypeBuilder<User> builder)
        {
        }

        protected override void ConfiguratePK(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(c => c.Id);
        }

        protected override void ConfigurateTableName(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("TbUsuarios");
        }
    }
}

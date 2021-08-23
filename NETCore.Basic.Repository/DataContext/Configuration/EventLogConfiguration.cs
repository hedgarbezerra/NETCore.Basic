using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Util.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Repository.DataContext.Configuration
{
    public sealed class EventLogConfiguration : DataConfiguration<EventLog>
    {
        public EventLogConfiguration(IConfiguration config)
            :base()
        {
            _config = config;
        }

        public IConfiguration _config { get; }

        protected override void ConfigurateFields(EntityTypeBuilder<EventLog> builder)
        {
            builder.Property(c => c.Id)
                .HasColumnType("int")
                .HasColumnName("Id");

            builder.Property(c => c.Message)
                .HasColumnType("nvarchar")
                .HasColumnName("Message");

            builder.Property(c => c.MessageTemplate)
                .HasColumnType("nvarchar")
                .HasColumnName("MessageTemplate");

            builder.Property(c => c.Exception)
                .HasColumnType("nvarchar")
                .HasColumnName("Exception");

            builder.Property(c => c.CreatedTime)
                .HasColumnType("datetime")
                .HasColumnName("TimeStamp");

            builder.Property(c => c.LogLevel)
                .HasColumnType("nvarchar")
                .HasColumnName("Level");

            builder.Property(c => c.Properties)
                .HasColumnType("nvarchar")
                .HasColumnName("Properties");

            builder.Ignore(c => c.XmlContent);
        }

        protected override void ConfigurateFK(EntityTypeBuilder<EventLog> builder)
        {
        }

        protected override void ConfiguratePK(EntityTypeBuilder<EventLog> builder)
        {
            builder.HasKey(c => c.Id);
        }

        protected override void ConfigurateTableName(EntityTypeBuilder<EventLog> builder)
        {
            builder.ToTable(_config["Logging:Table"]);
        }
    }
}

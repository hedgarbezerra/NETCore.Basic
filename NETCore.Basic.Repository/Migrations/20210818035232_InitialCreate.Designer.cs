﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NETCore.Basic.Repository.DataContext;

namespace NETCore.Basic.Repository.Migrations
{
    [DbContext(typeof(NetDbContext))]
    [Migration("20210818035232_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NETCore.Basic.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<DateTime>("RegistredAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValue(new DateTime(2021, 8, 18, 0, 52, 32, 452, DateTimeKind.Local).AddTicks(9215));

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.HasKey("Id");

                    b.ToTable("TbUsuarios");
                });
#pragma warning restore 612, 618
        }
    }
}

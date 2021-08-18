using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCore.Basic.Repository.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbUsuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Username = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    Password = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    RegistredAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(2021, 8, 18, 0, 52, 32, 452, DateTimeKind.Local).AddTicks(9215))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUsuarios", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbUsuarios");
        }
    }
}

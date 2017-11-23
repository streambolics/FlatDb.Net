using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace flatdatabase.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data0 = table.Column<string>(type: "TEXT", nullable: true),
                    Data1 = table.Column<string>(type: "TEXT", nullable: true),
                    Data2 = table.Column<string>(type: "TEXT", nullable: true),
                    Data3 = table.Column<string>(type: "TEXT", nullable: true),
                    Data4 = table.Column<string>(type: "TEXT", nullable: true),
                    Data5 = table.Column<string>(type: "TEXT", nullable: true),
                    Data6 = table.Column<string>(type: "TEXT", nullable: true),
                    Data7 = table.Column<string>(type: "TEXT", nullable: true),
                    DetailId = table.Column<string>(type: "TEXT", nullable: true),
                    ItemId = table.Column<string>(type: "TEXT", nullable: true),
                    MasterId = table.Column<string>(type: "TEXT", nullable: true),
                    StatusId = table.Column<string>(type: "TEXT", nullable: true),
                    SubTypeId = table.Column<string>(type: "TEXT", nullable: true),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    TypeId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Item");
        }
    }
}

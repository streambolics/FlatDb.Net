using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace flatdatabase.Migrations
{
    public partial class Stepping1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Amount",
                table: "Item",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Database",
                table: "Item",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Item",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "Sequence",
                table: "Item",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Item",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Database",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Item");
        }
    }
}

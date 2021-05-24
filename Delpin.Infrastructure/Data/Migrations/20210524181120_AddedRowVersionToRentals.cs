using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Delpin.Infrastructure.Data.Migrations
{
    public partial class AddedRowVersionToRentals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Rentals",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Rentals");
        }
    }
}

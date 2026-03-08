using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToyotaWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddTestDateAndTimeSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TestDate",
                table: "DangKyLaiThus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TimeSlot",
                table: "DangKyLaiThus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestDate",
                table: "DangKyLaiThus");

            migrationBuilder.DropColumn(
                name: "TimeSlot",
                table: "DangKyLaiThus");
        }
    }
}

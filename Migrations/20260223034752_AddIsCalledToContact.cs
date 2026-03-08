using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToyotaWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCalledToContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCalled",
                table: "Contacts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCalled",
                table: "Contacts");
        }
    }
}

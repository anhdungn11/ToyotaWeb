using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToyotaWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddCallNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CallNote",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CallNote",
                table: "Contacts");
        }
    }
}

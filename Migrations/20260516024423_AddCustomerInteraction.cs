using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToyotaWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerInteraction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "CompanyExpenses");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "CustomerInteractions",
                newName: "Type");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeName",
                table: "EmployeeSalaries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "Allowance",
                table: "EmployeeSalaries",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Branch",
                table: "EmployeeSalaries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CarsSold",
                table: "EmployeeSalaries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "EmployeeSalaries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Insurance",
                table: "EmployeeSalaries",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NetSalary",
                table: "EmployeeSalaries",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PersonalTax",
                table: "EmployeeSalaries",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "EmployeeSalaries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "EmployeeSalaries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalRevenue",
                table: "EmployeeSalaries",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextFollowUpDate",
                table: "CustomerInteractions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExpenseName",
                table: "CompanyExpenses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "CompanyExpenses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Allowance",
                table: "EmployeeSalaries");

            migrationBuilder.DropColumn(
                name: "Branch",
                table: "EmployeeSalaries");

            migrationBuilder.DropColumn(
                name: "CarsSold",
                table: "EmployeeSalaries");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "EmployeeSalaries");

            migrationBuilder.DropColumn(
                name: "Insurance",
                table: "EmployeeSalaries");

            migrationBuilder.DropColumn(
                name: "NetSalary",
                table: "EmployeeSalaries");

            migrationBuilder.DropColumn(
                name: "PersonalTax",
                table: "EmployeeSalaries");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "EmployeeSalaries");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "EmployeeSalaries");

            migrationBuilder.DropColumn(
                name: "TotalRevenue",
                table: "EmployeeSalaries");

            migrationBuilder.DropColumn(
                name: "NextFollowUpDate",
                table: "CustomerInteractions");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "CustomerInteractions",
                newName: "Status");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeName",
                table: "EmployeeSalaries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExpenseName",
                table: "CompanyExpenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "CompanyExpenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "CompanyExpenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

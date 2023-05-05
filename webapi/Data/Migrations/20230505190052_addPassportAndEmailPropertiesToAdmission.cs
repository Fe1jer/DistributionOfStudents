using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Data.Migrations
{
    public partial class addPassportAndEmailPropertiesToAdmission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Admissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassportID",
                table: "Admissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PassportNumber",
                table: "Admissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassportSeries",
                table: "Admissions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "PassportID",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "PassportNumber",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "PassportSeries",
                table: "Admissions");
        }
    }
}

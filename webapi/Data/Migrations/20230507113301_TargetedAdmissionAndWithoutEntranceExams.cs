using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Data.Migrations
{
    public partial class TargetedAdmissionAndWithoutEntranceExams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Target",
                table: "RecruitmentPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsOutOfCompetition",
                table: "Admissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTargeted",
                table: "Admissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWithoutEntranceExams",
                table: "Admissions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Target",
                table: "RecruitmentPlans");

            migrationBuilder.DropColumn(
                name: "IsOutOfCompetition",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "IsTargeted",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "IsWithoutEntranceExams",
                table: "Admissions");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistributionOfStudents.Data.Migrations
{
    public partial class CreateAdmissionsWithPriorities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdmissionId",
                table: "StudentScores",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Admissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    GroupOfSpecialtiesId = table.Column<int>(type: "int", nullable: false),
                    DateOfApplication = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admissions_GroupsOfSpecialties_GroupOfSpecialtiesId",
                        column: x => x.GroupOfSpecialtiesId,
                        principalTable: "GroupsOfSpecialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Admissions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialtyPriorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecruitmentPlanId = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    AdmissionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialtyPriorities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialtyPriorities_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialtyPriorities_RecruitmentPlans_RecruitmentPlanId",
                        column: x => x.RecruitmentPlanId,
                        principalTable: "RecruitmentPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentScores_AdmissionId",
                table: "StudentScores",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_GroupOfSpecialtiesId",
                table: "Admissions",
                column: "GroupOfSpecialtiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_StudentId",
                table: "Admissions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyPriorities_AdmissionId",
                table: "SpecialtyPriorities",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyPriorities_RecruitmentPlanId",
                table: "SpecialtyPriorities",
                column: "RecruitmentPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentScores_Admissions_AdmissionId",
                table: "StudentScores",
                column: "AdmissionId",
                principalTable: "Admissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentScores_Admissions_AdmissionId",
                table: "StudentScores");

            migrationBuilder.DropTable(
                name: "SpecialtyPriorities");

            migrationBuilder.DropTable(
                name: "Admissions");

            migrationBuilder.DropIndex(
                name: "IX_StudentScores_AdmissionId",
                table: "StudentScores");

            migrationBuilder.DropColumn(
                name: "AdmissionId",
                table: "StudentScores");
        }
    }
}

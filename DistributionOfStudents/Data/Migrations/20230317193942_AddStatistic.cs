using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistributionOfStudents.Data.Migrations
{
    public partial class AddStatistic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupsOfSpecialitiesStatistic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupOfSpecialtiesId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CountOfAdmissions = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupsOfSpecialitiesStatistic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupsOfSpecialitiesStatistic_GroupsOfSpecialties_GroupOfSpecialtiesId",
                        column: x => x.GroupOfSpecialtiesId,
                        principalTable: "GroupsOfSpecialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecruitmentPlandStatistic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecruitmentPlanId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruitmentPlandStatistic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecruitmentPlandStatistic_RecruitmentPlans_RecruitmentPlanId",
                        column: x => x.RecruitmentPlanId,
                        principalTable: "RecruitmentPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupsOfSpecialitiesStatistic_GroupOfSpecialtiesId",
                table: "GroupsOfSpecialitiesStatistic",
                column: "GroupOfSpecialtiesId");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentPlandStatistic_RecruitmentPlanId",
                table: "RecruitmentPlandStatistic",
                column: "RecruitmentPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupsOfSpecialitiesStatistic");

            migrationBuilder.DropTable(
                name: "RecruitmentPlandStatistic");
        }
    }
}

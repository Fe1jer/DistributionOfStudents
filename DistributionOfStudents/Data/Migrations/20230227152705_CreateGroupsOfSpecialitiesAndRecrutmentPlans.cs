using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistributionOfStudents.Data.Migrations
{
    public partial class CreateGroupsOfSpecialitiesAndRecrutmentPlans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupsOfSpecialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    IsDailyForm = table.Column<bool>(type: "bit", nullable: false),
                    IsBudget = table.Column<bool>(type: "bit", nullable: false),
                    IsFullTime = table.Column<bool>(type: "bit", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupsOfSpecialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecruitmentPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecialityId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    PassingScore = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    IsDailyForm = table.Column<bool>(type: "bit", nullable: false),
                    IsBudget = table.Column<bool>(type: "bit", nullable: false),
                    IsFullTime = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruitmentPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecruitmentPlans_Specialities_SpecialityId",
                        column: x => x.SpecialityId,
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupOfSpecialtiesSpeciality",
                columns: table => new
                {
                    GroupsOfSpecialtiesId = table.Column<int>(type: "int", nullable: false),
                    SpecialitiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupOfSpecialtiesSpeciality", x => new { x.GroupsOfSpecialtiesId, x.SpecialitiesId });
                    table.ForeignKey(
                        name: "FK_GroupOfSpecialtiesSpeciality_GroupsOfSpecialties_GroupsOfSpecialtiesId",
                        column: x => x.GroupsOfSpecialtiesId,
                        principalTable: "GroupsOfSpecialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupOfSpecialtiesSpeciality_Specialities_SpecialitiesId",
                        column: x => x.SpecialitiesId,
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupOfSpecialtiesSpeciality_SpecialitiesId",
                table: "GroupOfSpecialtiesSpeciality",
                column: "SpecialitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentPlans_SpecialityId",
                table: "RecruitmentPlans",
                column: "SpecialityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupOfSpecialtiesSpeciality");

            migrationBuilder.DropTable(
                name: "RecruitmentPlans");

            migrationBuilder.DropTable(
                name: "GroupsOfSpecialties");
        }
    }
}

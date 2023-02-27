using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistributionOfStudents.Data.Migrations
{
    public partial class CreateEnrolledStudents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnrolledStudents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    RecruitmentPlanId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrolledStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnrolledStudents_RecruitmentPlans_RecruitmentPlanId",
                        column: x => x.RecruitmentPlanId,
                        principalTable: "RecruitmentPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EnrolledStudents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnrolledStudents_RecruitmentPlanId",
                table: "EnrolledStudents",
                column: "RecruitmentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrolledStudents_StudentId",
                table: "EnrolledStudents",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnrolledStudents");
        }
    }
}

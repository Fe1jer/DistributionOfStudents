using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistributionOfStudents.Data.Migrations
{
    public partial class AddSubjectsToGroupsOfSpec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupOfSpecialtiesSubject",
                columns: table => new
                {
                    GroupsOfSpecialtiesId = table.Column<int>(type: "int", nullable: false),
                    SubjectsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupOfSpecialtiesSubject", x => new { x.GroupsOfSpecialtiesId, x.SubjectsId });
                    table.ForeignKey(
                        name: "FK_GroupOfSpecialtiesSubject_GroupsOfSpecialties_GroupsOfSpecialtiesId",
                        column: x => x.GroupsOfSpecialtiesId,
                        principalTable: "GroupsOfSpecialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupOfSpecialtiesSubject_Subjects_SubjectsId",
                        column: x => x.SubjectsId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupOfSpecialtiesSubject_SubjectsId",
                table: "GroupOfSpecialtiesSubject",
                column: "SubjectsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupOfSpecialtiesSubject");
        }
    }
}

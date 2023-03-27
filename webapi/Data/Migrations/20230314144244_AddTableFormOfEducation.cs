using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Data.Migrations
{
    public partial class AddTableFormOfEducation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBudget",
                table: "RecruitmentPlans");

            migrationBuilder.DropColumn(
                name: "IsDailyForm",
                table: "RecruitmentPlans");

            migrationBuilder.DropColumn(
                name: "IsFullTime",
                table: "RecruitmentPlans");

            migrationBuilder.DropColumn(
                name: "IsBudget",
                table: "GroupsOfSpecialties");

            migrationBuilder.DropColumn(
                name: "IsDailyForm",
                table: "GroupsOfSpecialties");

            migrationBuilder.DropColumn(
                name: "IsFullTime",
                table: "GroupsOfSpecialties");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "RecruitmentPlans",
                newName: "FormOfEducationId");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "GroupsOfSpecialties",
                newName: "FormOfEducationId");

            migrationBuilder.CreateTable(
                name: "FormsOfEducation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    IsDailyForm = table.Column<bool>(type: "bit", nullable: false),
                    IsBudget = table.Column<bool>(type: "bit", nullable: false),
                    IsFullTime = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormsOfEducation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentPlans_FormOfEducationId",
                table: "RecruitmentPlans",
                column: "FormOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsOfSpecialties_FormOfEducationId",
                table: "GroupsOfSpecialties",
                column: "FormOfEducationId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecruitmentPlans_FormsOfEducation_FormOfEducationId",
                table: "RecruitmentPlans",
                column: "FormOfEducationId",
                principalTable: "FormsOfEducation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupsOfSpecialties_FormsOfEducation_FormOfEducationId",
                table: "GroupsOfSpecialties",
                column: "FormOfEducationId",
                principalTable: "FormsOfEducation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupsOfSpecialties_FormsOfEducation_FormOfEducationId",
                table: "GroupsOfSpecialties");

            migrationBuilder.DropForeignKey(
                name: "FK_RecruitmentPlans_FormsOfEducation_FormOfEducationId",
                table: "RecruitmentPlans");

            migrationBuilder.DropTable(
                name: "FormsOfEducation");

            migrationBuilder.DropIndex(
                name: "IX_RecruitmentPlans_FormOfEducationId",
                table: "RecruitmentPlans");

            migrationBuilder.DropIndex(
                name: "IX_GroupsOfSpecialties_FormOfEducationId",
                table: "GroupsOfSpecialties");

            migrationBuilder.RenameColumn(
                name: "FormOfEducationId",
                table: "RecruitmentPlans",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "FormOfEducationId",
                table: "GroupsOfSpecialties",
                newName: "Year");

            migrationBuilder.AddColumn<bool>(
                name: "IsBudget",
                table: "RecruitmentPlans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDailyForm",
                table: "RecruitmentPlans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFullTime",
                table: "RecruitmentPlans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBudget",
                table: "GroupsOfSpecialties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDailyForm",
                table: "GroupsOfSpecialties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFullTime",
                table: "GroupsOfSpecialties",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

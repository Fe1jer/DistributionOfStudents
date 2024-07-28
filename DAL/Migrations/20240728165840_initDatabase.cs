using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class initDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Img = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormsOfEducation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    IsDailyForm = table.Column<bool>(type: "bit", nullable: false),
                    IsBudget = table.Column<bool>(type: "bit", nullable: false),
                    IsFullTime = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormsOfEducation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GPA = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Img = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecializationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecializationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    FacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specialities_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupsOfSpecialties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    FormOfEducationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupsOfSpecialties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupsOfSpecialties_FormsOfEducation_FormOfEducationId",
                        column: x => x.FormOfEducationId,
                        principalTable: "FormsOfEducation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecruitmentPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpecialityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Target = table.Column<int>(type: "int", nullable: false),
                    TargetPassingScore = table.Column<int>(type: "int", nullable: false),
                    PassingScore = table.Column<int>(type: "int", nullable: false),
                    FormOfEducationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruitmentPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecruitmentPlans_FormsOfEducation_FormOfEducationId",
                        column: x => x.FormOfEducationId,
                        principalTable: "FormsOfEducation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecruitmentPlans_Specialities_SpecialityId",
                        column: x => x.SpecialityId,
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Admissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupOfSpecialtiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateOfApplication = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PassportID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportSeries = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportNumber = table.Column<int>(type: "int", nullable: true),
                    IsTargeted = table.Column<bool>(type: "bit", nullable: false),
                    IsWithoutEntranceExams = table.Column<bool>(type: "bit", nullable: false),
                    IsOutOfCompetition = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "GroupOfSpecialitiesSpeciality",
                columns: table => new
                {
                    GroupsOfSpecialtiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpecialitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupOfSpecialitiesSpeciality", x => new { x.GroupsOfSpecialtiesId, x.SpecialitiesId });
                    table.ForeignKey(
                        name: "FK_GroupOfSpecialitiesSpeciality_GroupsOfSpecialties_GroupsOfSpecialtiesId",
                        column: x => x.GroupsOfSpecialtiesId,
                        principalTable: "GroupsOfSpecialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupOfSpecialitiesSpeciality_Specialities_SpecialitiesId",
                        column: x => x.SpecialitiesId,
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupOfSpecialitiesSubject",
                columns: table => new
                {
                    GroupsOfSpecialtiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupOfSpecialitiesSubject", x => new { x.GroupsOfSpecialtiesId, x.SubjectsId });
                    table.ForeignKey(
                        name: "FK_GroupOfSpecialitiesSubject_GroupsOfSpecialties_GroupsOfSpecialtiesId",
                        column: x => x.GroupsOfSpecialtiesId,
                        principalTable: "GroupsOfSpecialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupOfSpecialitiesSubject_Subjects_SubjectsId",
                        column: x => x.SubjectsId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupsOfSpecialitiesStatistic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupOfSpecialtiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                name: "EnrolledStudents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecruitmentPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "RecruitmentPlansStatistic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecruitmentPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruitmentPlansStatistic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecruitmentPlansStatistic_RecruitmentPlans_RecruitmentPlanId",
                        column: x => x.RecruitmentPlanId,
                        principalTable: "RecruitmentPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialtyPriorities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecruitmentPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    AdmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialtyPriorities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialtyPriorities_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SpecialtyPriorities_RecruitmentPlans_RecruitmentPlanId",
                        column: x => x.RecruitmentPlanId,
                        principalTable: "RecruitmentPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    AdmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentScores_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentScores_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_GroupOfSpecialtiesId",
                table: "Admissions",
                column: "GroupOfSpecialtiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_StudentId",
                table: "Admissions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrolledStudents_RecruitmentPlanId",
                table: "EnrolledStudents",
                column: "RecruitmentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrolledStudents_StudentId",
                table: "EnrolledStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupOfSpecialitiesSpeciality_SpecialitiesId",
                table: "GroupOfSpecialitiesSpeciality",
                column: "SpecialitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupOfSpecialitiesSubject_SubjectsId",
                table: "GroupOfSpecialitiesSubject",
                column: "SubjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsOfSpecialitiesStatistic_GroupOfSpecialtiesId",
                table: "GroupsOfSpecialitiesStatistic",
                column: "GroupOfSpecialtiesId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsOfSpecialties_FormOfEducationId",
                table: "GroupsOfSpecialties",
                column: "FormOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentPlans_FormOfEducationId",
                table: "RecruitmentPlans",
                column: "FormOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentPlans_SpecialityId",
                table: "RecruitmentPlans",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentPlansStatistic_RecruitmentPlanId",
                table: "RecruitmentPlansStatistic",
                column: "RecruitmentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_FacultyId",
                table: "Specialities",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyPriorities_AdmissionId",
                table: "SpecialtyPriorities",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyPriorities_RecruitmentPlanId",
                table: "SpecialtyPriorities",
                column: "RecruitmentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentScores_AdmissionId",
                table: "StudentScores",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentScores_SubjectId",
                table: "StudentScores",
                column: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnrolledStudents");

            migrationBuilder.DropTable(
                name: "GroupOfSpecialitiesSpeciality");

            migrationBuilder.DropTable(
                name: "GroupOfSpecialitiesSubject");

            migrationBuilder.DropTable(
                name: "GroupsOfSpecialitiesStatistic");

            migrationBuilder.DropTable(
                name: "RecruitmentPlansStatistic");

            migrationBuilder.DropTable(
                name: "SpecialtyPriorities");

            migrationBuilder.DropTable(
                name: "StudentScores");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "RecruitmentPlans");

            migrationBuilder.DropTable(
                name: "Admissions");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Specialities");

            migrationBuilder.DropTable(
                name: "GroupsOfSpecialties");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "FormsOfEducation");
        }
    }
}

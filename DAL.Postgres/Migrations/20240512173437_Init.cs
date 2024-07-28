using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "Faculties",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    ShortName = table.Column<string>(type: "text", nullable: false),
                    Img = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormsOfEducation",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    IsDailyForm = table.Column<bool>(type: "boolean", nullable: false),
                    IsBudget = table.Column<bool>(type: "boolean", nullable: false),
                    IsFullTime = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormsOfEducation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Patronymic = table.Column<string>(type: "text", nullable: false),
                    GPA = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Patronymic = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Img = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialities",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    ShortName = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: false),
                    ShortCode = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DirectionName = table.Column<string>(type: "text", nullable: true),
                    DirectionCode = table.Column<string>(type: "text", nullable: true),
                    SpecializationName = table.Column<string>(type: "text", nullable: true),
                    SpecializationCode = table.Column<string>(type: "text", nullable: true),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    FacultyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specialities_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalSchema: "public",
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupsOfSpecialties",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    FormOfEducationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupsOfSpecialties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupsOfSpecialties_FormsOfEducation_FormOfEducationId",
                        column: x => x.FormOfEducationId,
                        principalSchema: "public",
                        principalTable: "FormsOfEducation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecruitmentPlans",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SpecialityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    Target = table.Column<int>(type: "integer", nullable: false),
                    TargetPassingScore = table.Column<int>(type: "integer", nullable: false),
                    PassingScore = table.Column<int>(type: "integer", nullable: false),
                    FormOfEducationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruitmentPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecruitmentPlans_FormsOfEducation_FormOfEducationId",
                        column: x => x.FormOfEducationId,
                        principalSchema: "public",
                        principalTable: "FormsOfEducation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecruitmentPlans_Specialities_SpecialityId",
                        column: x => x.SpecialityId,
                        principalSchema: "public",
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Admissions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupOfSpecialtiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateOfApplication = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PassportID = table.Column<string>(type: "text", nullable: true),
                    PassportSeries = table.Column<string>(type: "text", nullable: true),
                    PassportNumber = table.Column<int>(type: "integer", nullable: true),
                    IsTargeted = table.Column<bool>(type: "boolean", nullable: false),
                    IsWithoutEntranceExams = table.Column<bool>(type: "boolean", nullable: false),
                    IsOutOfCompetition = table.Column<bool>(type: "boolean", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admissions_GroupsOfSpecialties_GroupOfSpecialtiesId",
                        column: x => x.GroupOfSpecialtiesId,
                        principalSchema: "public",
                        principalTable: "GroupsOfSpecialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Admissions_Students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "public",
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupOfSpecialitiesSpeciality",
                schema: "public",
                columns: table => new
                {
                    GroupsOfSpecialtiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpecialitiesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupOfSpecialitiesSpeciality", x => new { x.GroupsOfSpecialtiesId, x.SpecialitiesId });
                    table.ForeignKey(
                        name: "FK_GroupOfSpecialitiesSpeciality_GroupsOfSpecialties_GroupsOfS~",
                        column: x => x.GroupsOfSpecialtiesId,
                        principalSchema: "public",
                        principalTable: "GroupsOfSpecialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupOfSpecialitiesSpeciality_Specialities_SpecialitiesId",
                        column: x => x.SpecialitiesId,
                        principalSchema: "public",
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupOfSpecialitiesSubject",
                schema: "public",
                columns: table => new
                {
                    GroupsOfSpecialtiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupOfSpecialitiesSubject", x => new { x.GroupsOfSpecialtiesId, x.SubjectsId });
                    table.ForeignKey(
                        name: "FK_GroupOfSpecialitiesSubject_GroupsOfSpecialties_GroupsOfSpec~",
                        column: x => x.GroupsOfSpecialtiesId,
                        principalSchema: "public",
                        principalTable: "GroupsOfSpecialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupOfSpecialitiesSubject_Subjects_SubjectsId",
                        column: x => x.SubjectsId,
                        principalSchema: "public",
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupsOfSpecialitiesStatistic",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupOfSpecialtiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CountOfAdmissions = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupsOfSpecialitiesStatistic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupsOfSpecialitiesStatistic_GroupsOfSpecialties_GroupOfSp~",
                        column: x => x.GroupOfSpecialtiesId,
                        principalSchema: "public",
                        principalTable: "GroupsOfSpecialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnrolledStudents",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecruitmentPlanId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrolledStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnrolledStudents_RecruitmentPlans_RecruitmentPlanId",
                        column: x => x.RecruitmentPlanId,
                        principalSchema: "public",
                        principalTable: "RecruitmentPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EnrolledStudents_Students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "public",
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecruitmentPlansStatistic",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RecruitmentPlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruitmentPlansStatistic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecruitmentPlansStatistic_RecruitmentPlans_RecruitmentPlanId",
                        column: x => x.RecruitmentPlanId,
                        principalSchema: "public",
                        principalTable: "RecruitmentPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialtyPriorities",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RecruitmentPlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    AdmissionId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialtyPriorities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialtyPriorities_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalSchema: "public",
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialtyPriorities_RecruitmentPlans_RecruitmentPlanId",
                        column: x => x.RecruitmentPlanId,
                        principalSchema: "public",
                        principalTable: "RecruitmentPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentScores",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    AdmissionId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentScores_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalSchema: "public",
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentScores_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalSchema: "public",
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_GroupOfSpecialtiesId",
                schema: "public",
                table: "Admissions",
                column: "GroupOfSpecialtiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_StudentId",
                schema: "public",
                table: "Admissions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrolledStudents_RecruitmentPlanId",
                schema: "public",
                table: "EnrolledStudents",
                column: "RecruitmentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrolledStudents_StudentId",
                schema: "public",
                table: "EnrolledStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupOfSpecialitiesSpeciality_SpecialitiesId",
                schema: "public",
                table: "GroupOfSpecialitiesSpeciality",
                column: "SpecialitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupOfSpecialitiesSubject_SubjectsId",
                schema: "public",
                table: "GroupOfSpecialitiesSubject",
                column: "SubjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsOfSpecialitiesStatistic_GroupOfSpecialtiesId",
                schema: "public",
                table: "GroupsOfSpecialitiesStatistic",
                column: "GroupOfSpecialtiesId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsOfSpecialties_FormOfEducationId",
                schema: "public",
                table: "GroupsOfSpecialties",
                column: "FormOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentPlans_FormOfEducationId",
                schema: "public",
                table: "RecruitmentPlans",
                column: "FormOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentPlans_SpecialityId",
                schema: "public",
                table: "RecruitmentPlans",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentPlansStatistic_RecruitmentPlanId",
                schema: "public",
                table: "RecruitmentPlansStatistic",
                column: "RecruitmentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_FacultyId",
                schema: "public",
                table: "Specialities",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyPriorities_AdmissionId",
                schema: "public",
                table: "SpecialtyPriorities",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyPriorities_RecruitmentPlanId",
                schema: "public",
                table: "SpecialtyPriorities",
                column: "RecruitmentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentScores_AdmissionId",
                schema: "public",
                table: "StudentScores",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentScores_SubjectId",
                schema: "public",
                table: "StudentScores",
                column: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnrolledStudents",
                schema: "public");

            migrationBuilder.DropTable(
                name: "GroupOfSpecialitiesSpeciality",
                schema: "public");

            migrationBuilder.DropTable(
                name: "GroupOfSpecialitiesSubject",
                schema: "public");

            migrationBuilder.DropTable(
                name: "GroupsOfSpecialitiesStatistic",
                schema: "public");

            migrationBuilder.DropTable(
                name: "RecruitmentPlansStatistic",
                schema: "public");

            migrationBuilder.DropTable(
                name: "SpecialtyPriorities",
                schema: "public");

            migrationBuilder.DropTable(
                name: "StudentScores",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "public");

            migrationBuilder.DropTable(
                name: "RecruitmentPlans",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Admissions",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Subjects",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Specialities",
                schema: "public");

            migrationBuilder.DropTable(
                name: "GroupsOfSpecialties",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Students",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Faculties",
                schema: "public");

            migrationBuilder.DropTable(
                name: "FormsOfEducation",
                schema: "public");
        }
    }
}

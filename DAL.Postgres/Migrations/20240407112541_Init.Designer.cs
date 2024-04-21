﻿// <auto-generated />
using System;
using DAL.Postgres.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Postgres.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240407112541_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("ProductVersion", "6.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DAL.Postgres.Entities.Admission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateOfApplication")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<Guid>("GroupOfSpecialtiesId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsOutOfCompetition")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsTargeted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsWithoutEntranceExams")
                        .HasColumnType("boolean");

                    b.Property<string>("PassportID")
                        .HasColumnType("text");

                    b.Property<int?>("PassportNumber")
                        .HasColumnType("integer");

                    b.Property<string>("PassportSeries")
                        .HasColumnType("text");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("GroupOfSpecialtiesId");

                    b.HasIndex("StudentId");

                    b.ToTable("Admissions", "public");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.EnrolledStudent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("RecruitmentPlanId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RecruitmentPlanId");

                    b.HasIndex("StudentId");

                    b.ToTable("EnrolledStudents", "public");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.Faculty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Img")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Faculties", "public");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.FormOfEducation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsBudget")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDailyForm")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsFullTime")
                        .HasColumnType("boolean");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("FormsOfEducation", "public");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.GroupOfSpecialities", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("EnrollmentDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("FormOfEducationId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("FormOfEducationId");

                    b.ToTable("GroupsOfSpecialties", "public");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.GroupOfSpecialitiesStatistic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("CountOfAdmissions")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("GroupOfSpecialtiesId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("GroupOfSpecialtiesId");

                    b.ToTable("GroupsOfSpecialitiesStatistic", "public");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.RecruitmentPlan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<Guid>("FormOfEducationId")
                        .HasColumnType("uuid");

                    b.Property<int>("PassingScore")
                        .HasColumnType("integer");

                    b.Property<Guid>("SpecialityId")
                        .HasColumnType("uuid");

                    b.Property<int>("Target")
                        .HasColumnType("integer");

                    b.Property<int>("TargetPassingScore")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FormOfEducationId");

                    b.HasIndex("SpecialityId");

                    b.ToTable("RecruitmentPlans", "public");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.RecruitmentPlanStatistic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("RecruitmentPlanId")
                        .HasColumnType("uuid");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RecruitmentPlanId");

                    b.ToTable("RecruitmentPlandStatistic", "public");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.Speciality", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("DirectionCode")
                        .HasColumnType("text");

                    b.Property<string>("DirectionName")
                        .HasColumnType("text");

                    b.Property<Guid>("FacultyId")
                        .HasColumnType("uuid");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("boolean");

                    b.Property<string>("ShortCode")
                        .HasColumnType("text");

                    b.Property<string>("ShortName")
                        .HasColumnType("text");

                    b.Property<string>("SpecializationCode")
                        .HasColumnType("text");

                    b.Property<string>("SpecializationName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("Specialities", "public");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.SpecialityPriority", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AdmissionId")
                        .HasColumnType("uuid");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.Property<Guid>("RecruitmentPlanId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AdmissionId");

                    b.HasIndex("RecruitmentPlanId");

                    b.ToTable("SpecialtyPriorities", "public");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.Student", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("GPA")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Students", "public");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.StudentScore", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AdmissionId")
                        .HasColumnType("uuid");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AdmissionId");

                    b.HasIndex("SubjectId");

                    b.ToTable("StudentScores", "public");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.Subject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Subjects", "public");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Img")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users", "public");
                });

            modelBuilder.Entity("GroupOfSpecialitiesSpeciality", b =>
                {
                    b.Property<Guid>("GroupsOfSpecialtiesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SpecialitiesId")
                        .HasColumnType("uuid");

                    b.HasKey("GroupsOfSpecialtiesId", "SpecialitiesId");

                    b.HasIndex("SpecialitiesId");

                    b.ToTable("GroupOfSpecialitiesSpeciality", "public");
                });

            modelBuilder.Entity("GroupOfSpecialitiesSubject", b =>
                {
                    b.Property<Guid>("GroupsOfSpecialtiesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubjectsId")
                        .HasColumnType("uuid");

                    b.HasKey("GroupsOfSpecialtiesId", "SubjectsId");

                    b.HasIndex("SubjectsId");

                    b.ToTable("GroupOfSpecialitiesSubject", "public");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.Admission", b =>
                {
                    b.HasOne("DAL.Postgres.Entities.GroupOfSpecialities", "GroupOfSpecialties")
                        .WithMany("Admissions")
                        .HasForeignKey("GroupOfSpecialtiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Postgres.Entities.Student", "Student")
                        .WithMany("Admissions")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GroupOfSpecialties");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.EnrolledStudent", b =>
                {
                    b.HasOne("DAL.Postgres.Entities.RecruitmentPlan", null)
                        .WithMany("EnrolledStudents")
                        .HasForeignKey("RecruitmentPlanId");

                    b.HasOne("DAL.Postgres.Entities.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.GroupOfSpecialities", b =>
                {
                    b.HasOne("DAL.Postgres.Entities.FormOfEducation", "FormOfEducation")
                        .WithMany()
                        .HasForeignKey("FormOfEducationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FormOfEducation");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.GroupOfSpecialitiesStatistic", b =>
                {
                    b.HasOne("DAL.Postgres.Entities.GroupOfSpecialities", "GroupOfSpecialties")
                        .WithMany()
                        .HasForeignKey("GroupOfSpecialtiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GroupOfSpecialties");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.RecruitmentPlan", b =>
                {
                    b.HasOne("DAL.Postgres.Entities.FormOfEducation", "FormOfEducation")
                        .WithMany()
                        .HasForeignKey("FormOfEducationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Postgres.Entities.Speciality", "Speciality")
                        .WithMany("RecruitmentPlans")
                        .HasForeignKey("SpecialityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FormOfEducation");

                    b.Navigation("Speciality");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.RecruitmentPlanStatistic", b =>
                {
                    b.HasOne("DAL.Postgres.Entities.RecruitmentPlan", "RecruitmentPlan")
                        .WithMany()
                        .HasForeignKey("RecruitmentPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RecruitmentPlan");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.Speciality", b =>
                {
                    b.HasOne("DAL.Postgres.Entities.Faculty", "Faculty")
                        .WithMany("Specialities")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.SpecialityPriority", b =>
                {
                    b.HasOne("DAL.Postgres.Entities.Admission", null)
                        .WithMany("SpecialityPriorities")
                        .HasForeignKey("AdmissionId");

                    b.HasOne("DAL.Postgres.Entities.RecruitmentPlan", "RecruitmentPlan")
                        .WithMany()
                        .HasForeignKey("RecruitmentPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RecruitmentPlan");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.StudentScore", b =>
                {
                    b.HasOne("DAL.Postgres.Entities.Admission", null)
                        .WithMany("StudentScores")
                        .HasForeignKey("AdmissionId");

                    b.HasOne("DAL.Postgres.Entities.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("GroupOfSpecialitiesSpeciality", b =>
                {
                    b.HasOne("DAL.Postgres.Entities.GroupOfSpecialities", null)
                        .WithMany()
                        .HasForeignKey("GroupsOfSpecialtiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Postgres.Entities.Speciality", null)
                        .WithMany()
                        .HasForeignKey("SpecialitiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GroupOfSpecialitiesSubject", b =>
                {
                    b.HasOne("DAL.Postgres.Entities.GroupOfSpecialities", null)
                        .WithMany()
                        .HasForeignKey("GroupsOfSpecialtiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Postgres.Entities.Subject", null)
                        .WithMany()
                        .HasForeignKey("SubjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DAL.Postgres.Entities.Admission", b =>
                {
                    b.Navigation("SpecialityPriorities");

                    b.Navigation("StudentScores");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.Faculty", b =>
                {
                    b.Navigation("Specialities");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.GroupOfSpecialities", b =>
                {
                    b.Navigation("Admissions");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.RecruitmentPlan", b =>
                {
                    b.Navigation("EnrolledStudents");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.Speciality", b =>
                {
                    b.Navigation("RecruitmentPlans");
                });

            modelBuilder.Entity("DAL.Postgres.Entities.Student", b =>
                {
                    b.Navigation("Admissions");
                });
#pragma warning restore 612, 618
        }
    }
}

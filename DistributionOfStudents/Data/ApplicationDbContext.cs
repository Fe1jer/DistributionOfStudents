using DistributionOfStudents.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DistributionOfStudents.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudentScore> StudentScores { get; set; }
        public DbSet<GroupOfSpecialties> GroupsOfSpecialties { get; set; }
        public DbSet<RecruitmentPlan> RecruitmentPlans { get; set; }
        public DbSet<Admission> Admissions { get; set; }
        public DbSet<SpecialityPriority> SpecialtyPriorities { get; set; }
        public DbSet<EnrolledStudent> EnrolledStudents { get; set; }
        public DbSet<FormOfEducation> FormsOfEducation { get; set; }
        public DbSet<GroupOfSpecialitiesStatistic> GroupsOfSpecialitiesStatistic { get; set; }
        public DbSet<RecruitmentPlanStatistic> RecruitmentPlandStatistic { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Faculties = Set<Faculty>();
            Specialities = Set<Speciality>();
            Students = Set<Student>();
            Subjects = Set<Subject>();
            StudentScores = Set<StudentScore>();
            GroupsOfSpecialties = Set<GroupOfSpecialties>();
            RecruitmentPlans = Set<RecruitmentPlan>();
            Admissions = Set<Admission>();
            SpecialtyPriorities = Set<SpecialityPriority>();
            EnrolledStudents = Set<EnrolledStudent>();
            FormsOfEducation = Set<FormOfEducation>();
            GroupsOfSpecialitiesStatistic = Set<GroupOfSpecialitiesStatistic>();
            RecruitmentPlandStatistic = Set<RecruitmentPlanStatistic>();
        }
    }
}
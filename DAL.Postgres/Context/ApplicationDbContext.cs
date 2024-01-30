using DAL.Postgres.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Postgres.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudentScore> StudentScores { get; set; }
        public DbSet<GroupOfSpecialities> GroupsOfSpecialties { get; set; }
        public DbSet<RecruitmentPlan> RecruitmentPlans { get; set; }
        public DbSet<Admission> Admissions { get; set; }
        public DbSet<SpecialityPriority> SpecialtyPriorities { get; set; }
        public DbSet<EnrolledStudent> EnrolledStudents { get; set; }
        public DbSet<FormOfEducation> FormsOfEducation { get; set; }
        public DbSet<GroupOfSpecialitiesStatistic> GroupsOfSpecialitiesStatistic { get; set; }
        public DbSet<RecruitmentPlanStatistic> RecruitmentPlandStatistic { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            Users = Set<User>();
            Faculties = Set<Faculty>();
            Specialities = Set<Speciality>();
            Students = Set<Student>();
            Subjects = Set<Subject>();
            StudentScores = Set<StudentScore>();
            GroupsOfSpecialties = Set<GroupOfSpecialities>();
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
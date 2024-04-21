using DAL.Postgres.Context;
using DAL.Postgres.DBInitialization.Interface;
using DAL.Postgres.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Postgres.DBInitialization
{
    public class ApplicationDbContextInit
    {
        public static void InitDbContext(ApplicationDbContext? context)
        {
            if (context != null)
            {
                if (!context.Users.Any())
                {
                    CreateUsers(context);
                }
                if (!context.Subjects.Any())
                {
                    CreateSubjects(context);
                }
                if (!context.FormsOfEducation.Any())
                {
                    CreateFormsOfEducation(context);
                }
                if (!context.Faculties.Any())
                {
                    CreateFacultiesWithSpecialties(context);
                }
            }
        }

        private static void CreateFormsOfEducation(ApplicationDbContext context)
        {
            FormOfEducation form = new()
            {
                IsBudget = true,
                IsDailyForm = true,
                IsFullTime = true,
                Year = DateTime.Now.Year,
            };

            context.FormsOfEducation.Add(form);
            context.SaveChanges();
        }

        private static void CreateFacultiesWithSpecialties(ApplicationDbContext context)
        {
            List<Faculty> faculties = new();
            List<IFacultyInit> facultiesInit = new()
            {
                new ATFInit(), new FGDEInit(),new MSFInit(), new MTFInit(), new FMMP(), new EFInit(), new FITRInit(),

            };
            foreach (var facultyInit in facultiesInit)
            {
                Faculty faculty = facultyInit.GetFaculty();
                faculty.Specialities = facultyInit.GetSpecialties();
                faculties.Add(faculty);
                context.Faculties.Add(faculty);
                context.SaveChanges();
                faculty = context.Faculties.Include(f => f.Specialities).First(f => f.FullName == faculty.FullName);
                if (faculty.Specialities != null && faculty.Specialities.Count != 0)
                {
                    FormOfEducation form = context.FormsOfEducation.First();
                    List<RecruitmentPlan> plans = facultyInit.GetRecruitmentPlans(faculty.Specialities.Take(3).ToList(), form);
                    if (plans.Count != 0)
                    {
                        context.RecruitmentPlans.AddRange(plans);
                        context.SaveChanges();
                    }
                    List<GroupOfSpecialities> groups = facultyInit.GetGroupsOfSpecialties(faculty.Specialities.Take(3).ToList(), form);
                    if (groups.Count != 0)
                    {
                        context.GroupsOfSpecialties.AddRange(groups);
                        context.SaveChanges();
                    }
                }
            }

            context.SaveChanges();
        }

        private static void CreateSubjects(ApplicationDbContext context)
        {
            CreateSubject("русский/белорусский язык", context);
            CreateSubject("математика", context);
            CreateSubject("физика", context);
            CreateSubject("химия", context);
            CreateSubject("биология", context);
            CreateSubject("история Беларуси", context);
            CreateSubject("всемирная история", context);
            CreateSubject("обществоведение", context);
            CreateSubject("география", context);
            CreateSubject("английский язык", context);
            CreateSubject("французский язык", context);
            CreateSubject("немецкий язык", context);
            CreateSubject("китайский язык", context);
        }

        private static void CreateSubject(string name, ApplicationDbContext context)
        {
            Subject subject = new() { Name = name };
            context.Subjects.Add(subject);
            context.SaveChanges();
        }

        private static void CreateUsers(ApplicationDbContext context)
        {
            User admin = new()
            {
                UserName = "admin@gmail.com",
                Name = "Admin",
                Surname = "Admin",
                Patronymic = "Admin",
                Role = "admin",
                //adminDistribution
                PasswordHash = "APlb1LzG7drutYVMOyH/efSmFw0fKRnU6R4hBXzi6fAeN2k99IsepVTCrPZvtMt3mg==",
                Img = "\\img\\Users\\bntu.jpg"
            };
            context.Users.Add(admin);
            context.SaveChanges();

            User commission = new()
            {
                UserName = "commission@gmail.com",
                Name = "commission",
                Surname = "commission",
                Patronymic = "commission",
                Role = "commission",
                //commissionDistribution
                PasswordHash = "AH1/j12C/ytndGn2av/Mp8NJGkPHJygvpF8mMBNnR9E7xUWCcNPuKCBH7cnlOMIPIA==",
                Img = "\\img\\Users\\bntu.jpg"
            };
            context.Users.Add(commission);
            context.SaveChanges();
        }
    }
}

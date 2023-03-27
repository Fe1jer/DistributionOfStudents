using webapi.Data.DBInitialization.Interface;
using webapi.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace webapi.Data.DBInitialization
{
    public class ApplicationDbContextInit
    {
        public async static Task InitDbContextAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            /*context.Database.EnsureDeleted(); //Удалить базу данных
            context.Database.EnsureCreated(); //Создать базу данных, если она не существует на компьютере*/
            if (!await roleManager.Roles.AnyAsync())
            {
                await CreateRoles(roleManager);
            }
            if (!await userManager.Users.AnyAsync())
            {
                await CreateUsers(userManager);
            }
            if (!await context.Subjects.AnyAsync())
            {
                CreateSubjects(context);
            }
            if (!await context.FormsOfEducation.AnyAsync())
            {
                CreateFormsOfEducation(context);
            }
            if (!await context.Faculties.AnyAsync())
            {
                CreateFacultiesWithSpecialties(context);
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
                    List<GroupOfSpecialties> groups = facultyInit.GetGroupsOfSpecialties(faculty.Specialities.Take(3).ToList(), form);
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

        private async static Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole() { Name = "admission committee" });
            await roleManager.CreateAsync(new IdentityRole() { Name = "admin" });
        }

        private async static Task CreateUsers(UserManager<User> userManager)
        {
            User admin = new()
            {
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com",
                Name = "Admin",
                Surname = "Admin",
                Patronymic = "Admin",
                Img = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcScY-9qDVs2yQiXkeEHGQfvxEPLWHh-o53ZuQ&usqp=CAU"
            };
            await userManager.CreateAsync(admin, "adminPass_1");
            await userManager.AddToRoleAsync(admin, "admin");

            User admissioncommittee = new()
            {
                Email = "admissioncommittee@gmail.com",
                UserName = "admissioncommittee@gmail.com",
                Name = "admissioncommittee",
                Surname = "admissioncommittee",
                Patronymic = "admissioncommittee",
                Img = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcScY-9qDVs2yQiXkeEHGQfvxEPLWHh-o53ZuQ&usqp=CAU"
            };
            await userManager.CreateAsync(admissioncommittee, "admissionCommittee_1");
            await userManager.AddToRoleAsync(admissioncommittee, "admission committee");
        }
    }
}

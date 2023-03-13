using DistributionOfStudents.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DistributionOfStudents.Data
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
            if (!await context.Faculties.AnyAsync())
            {
                CreateFacultiesWithSpecialties(context);
            }
            if (!await context.RecruitmentPlans.AnyAsync())
            {
                CreateRecruitmentPlans(context);
            }
            if (!await context.GroupsOfSpecialties.AnyAsync())
            {
                CreateGroupOfSpecialties(context);
            }
        }

        private static void CreateRecruitmentPlans(ApplicationDbContext context)
        {
            RecruitmentPlan plan;

            foreach (Speciality specialty in context.Specialities.ToList())
            {
                plan = new RecruitmentPlan()
                {
                    Count = 20,
                    PassingScore = 0,
                    Speciality = specialty,
                    IsBudget = true,
                    IsDailyForm = true,
                    IsFullTime = true,
                    Year = DateTime.Today.Year
                };

                context.RecruitmentPlans.Add(plan);
                context.SaveChanges();
            };
        }

        private static void CreateGroupOfSpecialties(ApplicationDbContext context)
        {
            GroupOfSpecialties group = new()
            {
                Name = "Дневная форма получения образования за счет средств республиканского бюджета",
                StartDate = DateTime.Today,
                EnrollmentDate = DateTime.Today.AddDays(10),
                IsCompleted = false,
                IsBudget = true,
                IsDailyForm = true,
                IsFullTime = true,
                Year = DateTime.Today.Year,
                Specialities = context.Specialities.ToList()
            };

            context.GroupsOfSpecialties.Add(group);
            context.SaveChanges();
        }

        private static List<Speciality> CreateSpecialties()
        {
            Speciality poit = new()
            {
                FullName = "Программное обеспечение информационных технологий",
                ShortName = "ПОИТ",
                Code = "1-40 01 01",
                ShortCode = "01",
                SpecializationName = "Управление качеством и тестирование программного обеспечения",
                SpecializationCode = "1 40 01 01 05"
            };
            Speciality isitOPI = new()
            {
                FullName = "Информационные системы и технологии",
                ShortName = "ИСИТ",
                Code = "1-40 05 01",
                ShortCode = "02",
                SpecializationName = "Математическое обеспечение и системное программирование",
                SpecializationCode = "1 40 05 01 04 01",
                DirectionName = "Информационные системы и технологии в обработке и представлении информации",
                DirectionCode = "1-40 05 01-04"
            };
            Speciality isitPP = new()
            {
                FullName = "Информационные системы и технологии",
                ShortName = "ИСИТ",
                Code = "1-40 05 01",
                ShortCode = "03",
                DirectionName = "Информационные системы и технологии в проектировании и производстве",
                DirectionCode = "1-40 05 01-01"
            };

            return new List<Speciality>() { poit, isitOPI, isitPP };
        }

        private static void CreateFacultiesWithSpecialties(ApplicationDbContext context)
        {
            List<Speciality> specialties = CreateSpecialties();
            Faculty fitr = new()
            {
                Img = "\\img\\Faculties\\Default.jpg",
                FullName = "Факультет информационных технологий и робототехники",
                ShortName = "ФИТР",
                Specialities = specialties
            };

            context.Faculties.Add(fitr);
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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BLL.Services;
using BLL.Services.Interfaces;
using DAL.Postgres.Context;
using DAL.Postgres.Repositories;
using DAL.Postgres.Repositories.Interfaces;
using DAL.Postgres.DBInitialization;

namespace DependencyResolver
{
    public static class DependencyResolver
    {
        public static void RegisterApplicationServices(this IServiceCollection services, string connection)
        {
            // DbContext
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connection));

            services.AddTransient<ApplicationDbContext>();

            // Add application services.
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IFacultiesService, FacultiesService>();
            services.AddTransient<IAdmissionsService, AdmissionsService>();
            services.AddTransient<IStudentsService, StudentsService>();
            services.AddTransient<IGroupsOfSpecialitiesService, GroupsOfSpecialitiesService>();
            services.AddTransient<ISpecialitiesService, SpecialitiesService>();
            services.AddTransient<ISubjectsService, SubjectsService>();
            services.AddTransient<IRecruitmentPlansService, RecruitmentPlansService>();
            services.AddTransient<IFormsOfEducationService, FormsOfEducationService>();
            services.AddTransient<IGroupsOfSpecialitiesStatisticService, GroupsOfSpecialitiesStatisticService>();
            services.AddTransient<IRecruitmentPlansStatisticService, RecruitmentPlansStatisticService>();

        }

        public static void InitDatabase(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            ApplicationDbContext? prasDbContext = provider.GetService<ApplicationDbContext>();

            if (prasDbContext != null)
                prasDbContext.Database.Migrate();

            ApplicationDbContextInit.InitDbContext(prasDbContext);
        }
    }
}
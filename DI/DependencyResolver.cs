using BLL.Services;
using BLL.Services.Interfaces;
using DAL.Postgres.Context;
using DAL.Postgres.DBInitialization;
using DAL.Postgres.Repositories;
using DAL.Postgres.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DI
{
    public static class DependencyResolver
    {
        public static void RegisterApplicationServices(this IServiceCollection services, string connection)
        {
            // DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connection, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
                //options.EnableSensitiveDataLogging();
            });
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

            services.AddTransient<ApplicationDbContext>();

            // Add application services.
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IAdmissionsService, AdmissionsService>();
            services.AddTransient<IArchiveService, ArchiveService>();
            services.AddTransient<IDistributionService, DistributionService>();
            services.AddTransient<IFacultiesService, FacultiesService>();
            services.AddTransient<IGroupsOfSpecialitiesService, GroupsOfSpecialitiesService>();
            services.AddTransient<IRecruitmentPlansService, RecruitmentPlansService>();
            services.AddTransient<ISpecialitiesService, SpecialitiesService>();
            services.AddTransient<IStatisticService, StatisticService>();
            services.AddTransient<ISubjectsService, SubjectsService>();
            services.AddTransient<IUserService, UserService>();

        }

        public static void InitDatabase(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            ApplicationDbContext? dbContext = provider.GetService<ApplicationDbContext>();

            if (dbContext != null)
                dbContext.Database.Migrate();

            ApplicationDbContextInit.InitDbContext(dbContext);
        }
    }
}
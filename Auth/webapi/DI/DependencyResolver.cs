using BLL.Services;
using BLL.Services.Interfaces;
using DAL.Context;
using DAL.DBInitialization;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace webapi.DI
{
    public static class DependencyResolver
    {
        public static void RegisterApplicationServices(this IServiceCollection services, string connection)
        {
            // DbContext
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseNpgsql(connection, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
                //options.EnableSensitiveDataLogging();
            });
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

            services.AddTransient<AuthDbContext>();

            // Add application services.
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserTokenService, UserTokenService>();
            services.AddTransient<ICryptographyService, CryptographyService>();
            services.AddTransient<IJwtService, JwtService>();
        }

        public static void InitDatabase(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            AuthDbContext? dbContext = provider.GetService<AuthDbContext>();

            if (dbContext != null)
                dbContext.Database.Migrate();

            AuthDbContextInit.InitDbContext(dbContext);
        }
    }
}
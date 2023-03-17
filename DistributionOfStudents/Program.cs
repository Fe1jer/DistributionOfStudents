using DistributionOfStudents.Data;
using DistributionOfStudents.Data.DBInitialization;
using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DistributionOfStudents
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();
            await InitContext(app);

            Configure(app);
        }

        private async static Task InitContext(WebApplication app)
        {
            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            await ApplicationDbContextInit.InitDbContextAsync(userManager, roleManager, context);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
            builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            AddTransients(builder.Services);

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        private static void AddTransients(IServiceCollection services)
        {
            services.AddTransient<IFacultiesRepository, FacultiesRepository>();
            services.AddTransient<IAdmissionsRepository, AdmissionsRepository>();
            services.AddTransient<IStudentsRepository, StudentsRepository>();
            services.AddTransient<IGroupsOfSpecialitiesRepository, GroupsOfSpecialitiesRepository>();
            services.AddTransient<ISpecialitiesRepository, SpecialitiesRepository>();
            services.AddTransient<ISubjectsRepository, SubjectsRepository>();
            services.AddTransient<IRecruitmentPlansRepository, RecruitmentPlansRepository>();
            services.AddTransient<IFormsOfEducationRepository, FormsOfEducationRepository>();
            services.AddTransient<IGroupsOfSpecialitiesStatisticRepository, GroupsOfSpecialitiesStatisticRepository>();
            services.AddTransient<IRecruitmentPlansStatisticRepository, RecruitmentPlansStatisticRepository>();
        }
    }
}
using webapi.Data;
using webapi.Data.DBInitialization;
using webapi.Data.Models;
using webapi.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using webapi.Data.Interfaces.Repositories;

namespace webapi
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
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddCors();

            AddTransients(builder.Services);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

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
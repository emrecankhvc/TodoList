// Extensions/ServiceExtensions.cs - Tek dosya yeter!
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using TodoProject.Business.Interfaces;
using TodoProject.Business.Services;
using TodoProject.Data.Interfaces;
using TodoProject.Data.Repositories;
using TodoProject.Entities;
using TodoProject.Extensions;

namespace TodoProject.Extensions
{
    public static class ServiceExtensions
    {
        // Tüm DI kayıtlarını tek metotta topla
        public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            // MVC
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            // Database
            services.AddDbContext<DatabaseContext>(opts =>
            {
                opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            // Business Services
            services.AddScoped<ITodoService, TodoService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserNoteService, UserNoteService>();
            services.AddScoped<ICalendarService, CalendarService>();

            // Repositories
            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserNoteRepository, UserNoteRepository>();

            // Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opts =>
                {
                    opts.Cookie.Name = ".TodoProject.auth";
                    opts.ExpireTimeSpan = TimeSpan.FromDays(7);
                    opts.SlidingExpiration = true;
                    opts.LoginPath = "/Account/Login";
                    opts.LogoutPath = "/Account/Logout";
                    opts.AccessDeniedPath = "/Account/AccessDenied";
                });

            // Localization
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("tr"),
                    new CultureInfo("en")
                };
                options.DefaultRequestCulture = new RequestCulture("tr");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
            });
        }
    }
}


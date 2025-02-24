using AutoMapper;
using BusinessServiceLayer.Interfaces;
using BusinessServiceLayer.Services;
using Group1RazorPages.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;

namespace Group1RazorPages.Extensions
{
    public static class ApplicationServiceExtentsion
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            // Registers the database context with the DI container
            services.AddDbContext<FuNewsManagementContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register AutoMapper
            services.AddSingleton<IMapper>(sp => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            }).CreateMapper());

            // Register Services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<INewsArticleService, NewsArticleService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IReportService, ReportService>();

            // Configure Cookie
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Add Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            return services;
        }
    }
}

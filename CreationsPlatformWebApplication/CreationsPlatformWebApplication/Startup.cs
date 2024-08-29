using CreationsPlatformWebApplication.DataAccess.Contexts;
using CreationsPlatformWebApplication.DataAccess.Repositories;
using CreationsPlatformWebApplication.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CreationsPlatformWebApplication;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddAutoMapper(typeof(Startup).Assembly);

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        
        services.AddScoped<ICreationRepository, CreationRepository>();
        services.AddScoped<ICreationService, CreationService>();

        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IGenreService, GenreService>();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = new PathString(CookieAuthenticationDefaults.LoginPath);
                options.LogoutPath = new PathString(CookieAuthenticationDefaults.LogoutPath);
                options.AccessDeniedPath = new PathString(CookieAuthenticationDefaults.AccessDeniedPath);
                options.Cookie.HttpOnly = true;
            });
        services.AddOptions<MvcOptions>()
            .Configure<ILoggerFactory>((options, loggerFactory) =>
            {
                // options.ModelBinderProviders.Insert(0, new CustomCreationModelBinderProvider(loggerFactory));
            });
        services.AddMvc();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(name: "default",
                pattern: "{controller=CreationsPlatform}/{action=Index}/{id?}");
        });
    }
}
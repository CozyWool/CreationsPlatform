using CreationsPlatformWebApplication.DataAccess.Contexts;
using CreationsPlatformWebApplication.DataAccess.Repositories;
using CreationsPlatformWebApplication.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace CreationsPlatformWebApplication;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = new PathString(CookieAuthenticationDefaults.LoginPath);
                options.LogoutPath = new PathString(CookieAuthenticationDefaults.LogoutPath);
                options.AccessDeniedPath = new PathString(CookieAuthenticationDefaults.AccessDeniedPath);
                options.Cookie.HttpOnly = true;
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
using CreationsPlatformWebApplication.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CreationsPlatformWebApplication.DataAccess.Factories;

public class CreationsPlatformDbContextFactory : IDesignTimeDbContextFactory<CreationsPlatformDbContext>
{
    public CreationsPlatformDbContext CreateDbContext(string[] args)
    {
        const string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1;Database=CreationsPlatform;";
        var optionsBuilder = new DbContextOptionsBuilder<CreationsPlatformDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return new CreationsPlatformDbContext(optionsBuilder.Options);
    }
}
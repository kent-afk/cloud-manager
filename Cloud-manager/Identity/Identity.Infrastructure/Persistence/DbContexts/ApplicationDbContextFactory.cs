using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Identity.Infrastructure.Persistence.DbContexts;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder().SetBasePath(Path.Combine(Directory.GetCurrentDirectory()
            , "../Identity.API")).AddJsonFile("appsettings.Development.json").Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(config.GetConnectionString("Postgres")); 
        
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Hauto.Context;

public class HautoContextFactory : IDesignTimeDbContextFactory<HautoContext>
{
    public HautoContext CreateDbContext(string[] args)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json")
            .Build();
        var connectionString = config.GetConnectionString("Hauto");
        var optionsBuilder = new DbContextOptionsBuilder<HautoContext>();
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        return new HautoContext(optionsBuilder.Options);
    }
}
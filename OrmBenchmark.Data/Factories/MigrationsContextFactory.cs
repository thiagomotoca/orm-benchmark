using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OrmBenchmark.Data.Contexts;

namespace BenchmarkEFCoreDapper.Data.Factories;

public class MigrationsContextFactory : IDesignTimeDbContextFactory<EmployeeDbContext>
{
    public EmployeeDbContext CreateDbContext(string[] args)
    {
        return new EmployeeDbContext(GetOptions());
    }

    private DbContextOptions<EmployeeDbContext> GetOptions()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../OrmBenchmark"))
                .AddJsonFile("appsettings.json")
                .Build();

        var builder = new DbContextOptionsBuilder<EmployeeDbContext>();

        builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        return builder.Options;
    }
}
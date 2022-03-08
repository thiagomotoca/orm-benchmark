using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrmBenchmark.Data.Contexts;
using OrmBenchmark.Data.Interfaces;

namespace OrmBenchmark.Data.Factories;

public class EmployeeDbContextFactory : IEmployeeDbContextFactory
{
    private readonly IConfiguration _configuration;

    public EmployeeDbContextFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public EmployeeDbContext Create()
    {
        return new EmployeeDbContext(GetOptions());
    }

    public SqlConnection Connection()
    {
        return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
    }

    private DbContextOptions<EmployeeDbContext> GetOptions()
    {
        var builder = new DbContextOptionsBuilder<EmployeeDbContext>();

        builder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

        return builder.Options;
    }
}
using Microsoft.EntityFrameworkCore;
using OrmBenchmark.Data.Entities;

namespace OrmBenchmark.Data.Contexts;

public class EmployeeDbContext : DbContext
{
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
}
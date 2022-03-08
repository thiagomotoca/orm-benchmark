using Microsoft.Data.SqlClient;
using OrmBenchmark.Data.Contexts;

namespace OrmBenchmark.Data.Interfaces;

public interface IEmployeeDbContextFactory
{
    SqlConnection Connection();
    EmployeeDbContext Create();
}
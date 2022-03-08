using Bogus;
using Dapper;
using Microsoft.EntityFrameworkCore;
using OrmBenchmark.Data.Entities;
using OrmBenchmark.Data.Interfaces;

namespace OrmBenchmark.Data.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private static readonly Faker<Employee> fakeEmployees = new Faker<Employee>()
            .RuleFor(e => e.Name, f => f.Name.FullName())
            .RuleFor(e => e.Age, f => f.Random.Int(18, 50));

    private static readonly Faker<Department> fakeDepartments = new Faker<Department>()
            .RuleFor(d => d.DepartmentName, f => f.Commerce.Department())
            .RuleFor(d => d.Employees, f => fakeEmployees.Generate(f.Random.Int(1, 5)).ToList());
            
    private readonly IEmployeeDbContextFactory _employeeDBContextFactory;

    public EmployeeRepository(IEmployeeDbContextFactory employeeDBContextFactory)
    {
        _employeeDBContextFactory = employeeDBContextFactory;
    }

    public async Task InsertAsync()
    {
        var departments = BuildSampleData();

        using (var context = _employeeDBContextFactory.Create())
        {
            await context.Departments.AddRangeAsync(departments);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteAllAsync()
    {
        using (var connection = _employeeDBContextFactory.Connection())
        {
            await connection.ExecuteAsync("TRUNCATE TABLE Employees;");
            await connection.ExecuteAsync("DELETE FROM Departments;");
            await connection.ExecuteAsync("DBCC CHECKIDENT ([Departments], RESEED, 0)");
        }
    }

    public async Task<List<string>> GetFirstDepartmentEmployeesYoungerThan32WithEFCore()
    {
        using (var context = _employeeDBContextFactory.Create())
        {
            return await context.Employees
                .Where(e => e.Age < 32 && e.Department.Id == 1)
                .Select(s => s.Name)
                .ToListAsync();
        }
    }

    public async Task<List<string>> GetFirstDepartmentEmployeesYoungerThan32WithDapperAsync()
    {
        var cmd = @"SELECT e.Name
                    FROM Employees e WITH(NOLOCK)
                    INNER JOIN Departments d WITH(NOLOCK) on d.Id = e.DepartmentId
                    WHERE e.Age < 32 AND d.Id = 1";
        using (var connection = _employeeDBContextFactory.Connection())
        {
            var employeeNames = await connection.QueryAsync<string>(cmd);
            return employeeNames.ToList();
        }
    }

    public async Task InsertEmployeeWithEFCoreAsync()
    {
        var employee = fakeEmployees.Generate();
        employee.DepartmentId = 1;

        using (var context = _employeeDBContextFactory.Create())
        {
            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();
        }
    }

    public async Task InsertEmployeeWithDapperAsync()
    {
        var employee = fakeEmployees.Generate();
        employee.DepartmentId = 1;

        var cmd = "INSERT INTO Employees (Name, Age, DepartmentId) VALUES (@Name, @Age, @DepartmentId);";

        using (var connection = _employeeDBContextFactory.Connection())
        {
            await connection.ExecuteAsync(cmd, new { Name = employee.Name, Age = employee.Age, DepartmentId = 1 });
        }
    }

    private List<Department> BuildSampleData()
    {
        var rnd = new Random();
        return fakeDepartments.Generate(rnd.Next(3, 10));
    }
}
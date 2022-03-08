using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrmBenchmark.Data.Contexts;
using OrmBenchmark.Data.Factories;
using OrmBenchmark.Data.Interfaces;
using OrmBenchmark.Data.Repositories;

namespace OrmBenchmark;

[RankColumn]
[MemoryDiagnoser]
public class DatabaseBenchmark
{    
    private readonly IEmployeeRepository _employeeRepository;
    private const int numberOfIterations = 100;

    public DatabaseBenchmark()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

        var serviceProvider = new ServiceCollection()
                .AddDbContext<EmployeeDbContext>(builder => builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                .AddTransient<IEmployeeDbContextFactory, EmployeeDbContextFactory>()
                .AddTransient<IEmployeeRepository, EmployeeRepository>()
                .AddSingleton<IConfiguration>(configuration)
                .BuildServiceProvider();

        _employeeRepository = serviceProvider.GetService<IEmployeeRepository>();
    }

    [Benchmark]
    public async Task FetchUsingDapper()
    {
        for (int i = 0; i < numberOfIterations; i++)
        {
            await _employeeRepository.GetFirstDepartmentEmployeesYoungerThan32WithDapperAsync().ConfigureAwait(false);
        }
    }

    [Benchmark]
    public async Task FetchUsingEFCore()
    {
        for (int i = 0; i < numberOfIterations; i++)
        {
            await _employeeRepository.GetFirstDepartmentEmployeesYoungerThan32WithEFCore().ConfigureAwait(false);
        }
    }

    [Benchmark]
    public async Task InsertUsingDapper()
    {
        for (int i = 0; i < numberOfIterations; i++)
        {
            await _employeeRepository.InsertEmployeeWithDapperAsync().ConfigureAwait(false);
        }
    }

    [Benchmark]
    public async Task InsertUsingEFCore()
    {
        for (int i = 0; i < numberOfIterations; i++)
        {
            await _employeeRepository.InsertEmployeeWithEFCoreAsync().ConfigureAwait(false);
        }
    }
}
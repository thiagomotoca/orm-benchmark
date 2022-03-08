using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrmBenchmark;
using OrmBenchmark.Data.Contexts;
using OrmBenchmark.Data.Factories;
using OrmBenchmark.Data.Interfaces;
using OrmBenchmark.Data.Repositories;

IConfiguration configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

var serviceProvider = new ServiceCollection()
        .AddDbContext<EmployeeDbContext>(builder => builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
        .AddTransient<IEmployeeDbContextFactory, EmployeeDbContextFactory>()
        .AddTransient<IEmployeeRepository, EmployeeRepository>()
        .AddSingleton<IConfiguration>(configuration)
        .BuildServiceProvider();

var employeeRepository = serviceProvider.GetService<IEmployeeRepository>();

await employeeRepository.DeleteAllAsync();
await employeeRepository.InsertAsync();

var summary = BenchmarkRunner.Run<DatabaseBenchmark>();
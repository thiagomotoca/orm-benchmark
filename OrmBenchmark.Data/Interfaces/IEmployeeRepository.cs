namespace OrmBenchmark.Data.Interfaces;

public interface IEmployeeRepository
{
    Task DeleteAllAsync();
    Task InsertAsync();
    Task<List<string>> GetFirstDepartmentEmployeesYoungerThan32WithDapperAsync();
    Task<List<string>> GetFirstDepartmentEmployeesYoungerThan32WithEFCore();
    Task InsertEmployeeWithDapperAsync();
    Task InsertEmployeeWithEFCoreAsync();
}
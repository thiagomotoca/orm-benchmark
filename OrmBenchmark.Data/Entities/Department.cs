using System.ComponentModel.DataAnnotations;

namespace OrmBenchmark.Data.Entities;

public class Department
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string DepartmentName { get; set; }

    public virtual ICollection<Employee> Employees { get; set; }
}
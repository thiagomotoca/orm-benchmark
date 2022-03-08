using System.ComponentModel.DataAnnotations;

namespace OrmBenchmark.Data.Entities;

public class Employee
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    public int Age { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    public virtual Department Department { get; set; }
}
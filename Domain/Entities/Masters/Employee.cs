using Domain.Abstracts;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Masters;

[Table("tbmemployee")]
public class Employee : AuditTrail
{
    [Required]
    [StringLength(30)]
    public string Code { get; set; } = null!;
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
    [Required]
    public Guid PositionKey { get; set; }
    [Required]
    public Guid TitleKey { get; set; }
    [Required]
    public Guid GradeKey { get; set; }
    [Required]
    public EmployeeStatus Status { get; set; }
    public Guid? DirectSupervisorKey { get; set; } = Guid.Empty;
    [Required]
    public DateTime HireDate { get; set; }

    [NotMapped]
    public Position? Position { get; set; }
    [NotMapped]
    public Title? Title { get; set; }
    [NotMapped]
    public Grade? Grade { get; set; }
    [NotMapped]
    public Employee? DirectSupervisor { get; set; }
}

using Domain.Abstracts;
using Domain.Entities.Masters;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Transactions;

[Table("tbtovertime")]
public class Overtime : AuditTrail
{
    [Required]
    public Guid EmployeeKey { get; set; }
    [Required]
    public Guid ApprovalTransactionKey { get; set; }
    [Required]
    public DateOnly DateSubmission { get; set; }
    [Required]
    public TimeOnly OvertimeIn { get; set; }
    [Required]
    public TimeOnly OvertimeOut { get; set; }
    [Required]
    public ApprovalStatus ApprovalStatus { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; } = String.Empty;
    [Required]
    [MaxLength(30)]
    public string Number { get; set; } = null!;
    public TimeOnly? TimeIn { get; set; } = TimeOnly.MinValue;
    public TimeOnly? TimeOut { get; set; } = TimeOnly.MinValue;

    [NotMapped]
    public Employee? Employee { get; set; }
    [NotMapped]
    public ApprovalTransaction? ApprovalTransaction { get; set; }
}

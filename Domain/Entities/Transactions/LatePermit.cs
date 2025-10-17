using Domain.Abstracts;
using Domain.Entities.Masters;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Transactions;

[Table("tbtlatepermit")]
public class LatePermit : AuditTrail
{
    [Required]
    public Guid EmployeeKey { get; set; }
    [Required]
    public DateOnly DateSubmission { get; set; }
    [Required]
    public TimeOnly TimeIn { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; } = String.Empty;
    [Required]
    public ApprovalStatus ApprovalStatus { get; set; }
    [Required]
    [MaxLength(30)]
    public string Number { get; set; } = null!;

    [Required]
    public Guid ApprovalTransactionKey { get; set; }

    [NotMapped]
    public Employee? Employee { get; set; }
    [NotMapped]
    public ApprovalTransaction? ApprovalTransaction { get; set; }
}

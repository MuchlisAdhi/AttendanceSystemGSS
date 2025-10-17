using Domain.Abstracts;
using Domain.Entities.Masters;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Transactions;

[Table("tbtapprovalstamp")]
public class ApprovalStamp : AuditTrail
{
    [Required]
    public Guid ApprovalTransactionKey { get; set; }
    [Required]
    public Guid EmployeeKey { get; set; }
    [Required]
    public int Level { get; set; }
    [Required]
    [MaxLength(20)]
    public string Action { get; set; } = null!;
    [Required]
    public ApprovalStatus Status { get; set; }
    [MaxLength(200)]
    public string? RejectReason { get; set; } = String.Empty;
    [Required]
    public DateTime DateStamp { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [NotMapped]
    public ApprovalTransaction? ApprovalTransaction { get; set; }
    [NotMapped]
    public Employee? Approver { get; set; }
}

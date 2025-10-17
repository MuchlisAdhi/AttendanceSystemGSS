using Domain.Abstracts;
using Domain.Entities.Masters;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Transactions;

[Table("tbtapprovaltransaction")]
public class ApprovalTransaction : AuditTrail
{
    [Required]
    public Guid EmployeeKey { get; set; }
    [Required]
    public DateTime ApprovalTransactionDate { get; set; }
    [Required]
    public ApprovalCategory Category { get; set; }
    [Required]
    public ApprovalStatus ApprovalStatus { get; set; }
    [MaxLength(200)]
    public string? RejectReason { get; set; } = String.Empty;
    [MaxLength(200)]
    public string? Description { get; set; } = String.Empty;

    [NotMapped]
    public Employee? Submitter { get; set; }
    [NotMapped]
    public IEnumerable<ApprovalStamp>? ApprovalStamps { get; set; } = Enumerable.Empty<ApprovalStamp>();
}

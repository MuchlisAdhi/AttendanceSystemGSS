using Domain.Abstracts;
using Domain.Entities.Masters;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Transactions;

[Table("tbtleavesubmission")]
public class LeaveSubmission : AuditTrail
{
    [Required]
    public Guid EmployeeKey { get; set; }
    [Required]
    public Guid LeaveKey { get; set; }
    [Required]
    public DateTime DateStart { get; set; }
    [Required]
    public DateTime DateEnd { get; set; }
    [Required]
    public int Duration { get; set; }
    [Required]
    public ApprovalStatus ApprovalStatus { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; } = String.Empty;

    [Required]
    [MaxLength(10)]
    public string LeaveCode { get; set; } = null!;

    [Required]
    [MaxLength(30)]
    public string Number { get; set; } = null!;
    [Required]
    public Guid ApprovalTransactionKey { get; set; }
    [NotMapped]
    public Employee? Employee { get; set; }
    [NotMapped]
    public Leave? Leave { get; set; }
    [NotMapped]
    public ApprovalTransaction? ApprovalTransaction { get; set; }
}

using Domain.Abstracts;
using Domain.Entities.Masters;
using Domain.Enums;
using Domain.ViewModels.Transactions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Transactions;

[Table("tbtearlyout")]
public class EarlyOut : AuditTrail
{
    [Required]
    public Guid EmployeeKey { get; set; }
    [Required]
    public Guid ApprovalTransactionKey { get; set; }
    [Required]
    [MaxLength(30)]
    public string Number { get; set; } = null!;
    [Required]
    public DateOnly DateSubmission { get; set; }
    [Required]
    public TimeOnly TimeOut { get; set; }
    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = null!;
    [Required]
    public ApprovalStatus ApprovalStatus { get; set; }

    [NotMapped]
    public Employee? Employee { get; set; }
    [NotMapped]
    public ApprovalTransaction? ApprovalTransaction { get; set; }

    public EarlyOutPermitForm ConvertToViewModelEarlyOutPermit()
    {
        return new EarlyOutPermitForm
        {
            Key = this.Key,
            EmployeeKey = this.EmployeeKey,
            ApprovalTransactionKey = this.ApprovalTransactionKey,
            Number = this.Number,
            DateSubmission = this.DateSubmission,
            TimeOut = this.TimeOut,
            Description = this.Description,
            ApprovalStatus = this.ApprovalStatus,
            Employee = this.Employee
        };
    }
}

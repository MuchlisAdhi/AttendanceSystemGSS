using Domain.Dtos.Transactions;
using Domain.Entities.Masters;
using Domain.Entities.Transactions;
using Domain.Enums;

namespace Domain.ViewModels.Transactions;

public class ApprovalStampListItem
{
    public Guid Key { get; set; }
    public Guid? ApprovalTransactionKey { get; set; } = Guid.Empty;
    public Guid? EmployeeKey { get; set; } = Guid.Empty;
    public int Level { get; set; }
    public string Action { get; set; } = String.Empty;
    public ApprovalStatus? Status { get; set; }
    public string? StatusName { get; set; }
    public string? RejectReason { get; set; } = String.Empty;
    public bool CanApprove { get; set; }
    public bool CanReject { get; set; }
    public DateTime? DateStamp { get; set; }
    public string Email { get; set; } = null!;
    public ApprovalTransaction? ApprovalTransaction { get; set; }
    public Employee? Approver { get; set; }
}

public class ApprovalStampForm : ApprovalStampListItem
{
    //public List<SelectListItem> Employees { get; set; } = new List<SelectListItem>();

    public ApprovalStampDto ConvertToApprovalStampDto()
    {
        return new ApprovalStampDto
        {
            Key = this.Key,
            ApprovalTransactionKey = this.ApprovalTransactionKey,
            EmployeeKey = this.EmployeeKey,
            Level = this.Level,
            Action = this.Action,
            Status = this.Status,
            DateStamp = this.DateStamp,
            RejectReason = this.RejectReason,
            Email = this.Email
        };
    }
}

public class ApprovalStatusItemList
{
    public int No { get; set; }
    public string Approver { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string StatusName { get; set; } = null!;
    public ApprovalStatus? Status { get; set; } = ApprovalStatus.Waiting;
    public DateOnly? ApprovalDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public Guid? ApproverKey { get; set; }
    public int Level { get; set; }
    public string Email { get; set; } = null!;
}

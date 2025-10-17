using Domain.Dtos.Transactions;
using Domain.Entities.Masters;
using Domain.Enums;

namespace Domain.ViewModels.Transactions;

public class ApprovalTransactionListItem
{
    public Guid Key { get; set; }
    public Guid? EmployeeKey { get; set; } = Guid.Empty;
    public DateTime ApprovalTransactionDate { get; set; }
    public Employee? Submitter { get; set; }
    public ApprovalCategory? Category { get; set; }
    public string? CategoryName { get; set; } = string.Empty;
    public ApprovalStatus? ApprovalStatus { get; set; }
    public string? ApprovalStatusName { get; set; } = string.Empty;
    public string? RejectReason { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
}

public class ApprovalRequestItemList
{
    public Guid TransactionKey { get; set; }
    public Guid SubmitterKey { get; set; }
    public int CurrentLevel { get; set; }
    public int? MaxLevel { get; set; }
    public int? RejectedLevel { get; set; }
    public ApprovalTransactionForm? ApprovalTransaction { get; set; }
}

public class ApprovalTransactionForm : ApprovalTransactionListItem
{
    public IEnumerable<ApprovalStampForm>? ApprovalStamps { get; set; }

    //For Deserialization or Serialization Input form array
    public string JsonApprovalStamps { get; set; } = string.Empty;

    public ApprovalTransactionDto ConvertToApprovalTransactionDto()
    {
        return new ApprovalTransactionDto
        {
            Key = this.Key,
            EmployeeKey = this.EmployeeKey ?? Guid.Empty,
            ApprovalTransactionDate = this.ApprovalTransactionDate,
            Category = this.Category,
            ApprovalStatus = this.ApprovalStatus,
            RejectReason = this.RejectReason,
            Description = this.Description,
            ApprovalStamps = this.ApprovalStamps?.
                            Select(x => x.ConvertToApprovalStampDto()).ToList()
        };
    }
}

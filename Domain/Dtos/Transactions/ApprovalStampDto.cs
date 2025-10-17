using Domain.Entities.Transactions;
using Domain.Enums;

namespace Domain.Dtos.Transactions;

public class ApprovalStampDto
{
    public Guid? Key { get; set; } = Guid.Empty;
    public Guid? ApprovalTransactionKey { get; set; } = Guid.Empty;
    public Guid? EmployeeKey { get; set; } = Guid.Empty;
    public int Level { get; set; }
    public string Action { get; set; } = null!;
    public ApprovalStatus? Status { get; set; }
    public string? RejectReason { get; set; } = String.Empty;
    public DateTime? DateStamp { get; set; }
    public string Email { get; set; } = null!;

    public ApprovalStamp ConvertToEntity()
    {
        return new ApprovalStamp
        {
            Key = this.Key ?? Guid.Empty,
            ApprovalTransactionKey = this.ApprovalTransactionKey ?? Guid.Empty,
            EmployeeKey = this.EmployeeKey ?? Guid.Empty,
            Level = this.Level,
            Action = this.Action ?? String.Empty,
            DateStamp = this.DateStamp.HasValue ? this.DateStamp.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime(),
            Status = this.Status ?? Enums.ApprovalStatus.Waiting,
            RejectReason = this.RejectReason ?? String.Empty,
            Email = this.Email
        };
    }
}

using Domain.Entities.Transactions;
using Domain.Enums;

namespace Domain.Dtos.Transactions;

public class LeaveSubmissionDto
{
    public Guid? Key { get; set; } = Guid.Empty;
    public Guid? EmployeeKey { get; set; } = Guid.Empty;
    public Guid? LeaveKey { get; set; } = Guid.Empty;
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public int? Duration { get; set; }
    public ApprovalStatus? ApprovalStatus { get; set; } = Enums.ApprovalStatus.New;
    public string? Description { get; set; } = string.Empty;
    public string LeaveCode { get; set; } = null!;
    public string Number { get; set; } = null!;
    public Guid? ApprovalTransactionKey { get; set; }
    public IEnumerable<ApprovalStatusDto>? ApprovalStatuses { get; set; } = Enumerable.Empty<ApprovalStatusDto>();

    public LeaveSubmission ConvertToEntity()
    {
        return new LeaveSubmission
        {
            Key = this.Key ?? Guid.Empty,
            EmployeeKey = this.EmployeeKey ?? Guid.Empty,
            LeaveKey = this.LeaveKey ?? Guid.Empty,
            DateStart = this.DateStart ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 08, 00, 00),
            DateEnd = this.DateEnd ?? DateTime.Now.AddDays(1).AddTicks(-1),
            Duration = this.Duration ?? 0,
            ApprovalStatus = this.ApprovalStatus ?? Enums.ApprovalStatus.New,
            Description = this.Description ?? String.Empty,
            LeaveCode = this.LeaveCode,
            Number = this.Number,
            ApprovalTransactionKey = this.ApprovalTransactionKey ?? Guid.Empty
        };
    }
}

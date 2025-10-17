using Domain.Dtos.Transactions;
using Domain.Entities.Masters;
using Domain.Entities.Transactions;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Domain.ViewModels.Transactions;

public class LeaveSubmissionListItem
{
    public Guid Key { get; set; }
    public Guid? EmployeeKey { get; set; } = Guid.Empty;
    public Guid? LeaveKey { get; set; } = Guid.Empty;
    public DateTime? DateStart { get; set; } = DateTime.Now;
    public DateTime? DateEnd { get; set; } = DateTime.Now;
    public int? Duration { get; set; }
    public ApprovalStatus? ApprovalStatus { get; set; }
    public string? StatusName { get; set; }
    public string Number { get; set; } = null!;
    public string? LeaveCategory { get; set; }
    public Guid? ApprovalTransactionKey { get; set; } = Guid.Empty;
    public Employee? Employee { get; set; }
    public Leave? Leave { get; set; }
}

public class LeaveSubmissionList
{
    public IEnumerable<LeaveSubmissionListItem> LeaveSubmissions { get; set; } = new List<LeaveSubmissionListItem>();
}

public class LeaveSubmissionForm : LeaveSubmissionListItem
{
    public List<SelectListItem> Employees { get; set; } = new List<SelectListItem>();
    public string? Description { get; set; } = String.Empty;

    public string LeaveCode { get; set; } = null!;

    public ApprovalTransaction? ApprovalTransaction { get; set; }
    public IEnumerable<ApprovalStatusItemList> ApprovalStatuses { get; set; } = Enumerable.Empty<ApprovalStatusItemList>();

    public string JsonApprovalStatuses { get; set; } = string.Empty;

    public LeaveSubmissionDto ConvertToLeaveSubmissionDto()
    {
        return new LeaveSubmissionDto
        {
            Key = this.Key,
            EmployeeKey = this.EmployeeKey,
            LeaveKey = this.LeaveKey,
            LeaveCode = this.LeaveCode,
            DateStart = this.DateStart,
            DateEnd = this.DateEnd,
            Duration = this.Duration,
            ApprovalStatus = this.ApprovalStatus,
            Description = this.Description,
            Number = this.Number,
            ApprovalTransactionKey = this.ApprovalTransactionKey,
            ApprovalStatuses = this.ConvertToApprovalStatusDto()
        };
    }

    public IEnumerable<ApprovalStatusDto> ConvertToApprovalStatusDto()
    {
        if (ApprovalStatuses == null || !ApprovalStatuses.Any())
            return Enumerable.Empty<ApprovalStatusDto>();

        return ApprovalStatuses.Select(x => new ApprovalStatusDto
        {
            Action = x.Action,
            Status = x.Status,
            ApproverKey = x.ApproverKey,
            Level = x.Level,
            Email = x.Email
        });
    }
}

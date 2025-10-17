using Domain.Dtos.Transactions;
using Domain.Entities.Masters;
using Domain.Entities.Transactions;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Domain.ViewModels.Transactions;

public class LatePermitListItem
{
    public Guid Key { get; set; }
    public Guid? EmployeeKey { get; set; } = Guid.Empty;
    public DateOnly? DateSubmission { get; set; }
    public TimeOnly? TimeIn { get; set; }
    public string? Description { get; set; } = String.Empty;
    public ApprovalStatus? ApprovalStatus { get; set; }
    public string? StatusName { get; set; }
    public string Number { get; set; } = null!;
    public Employee? Employee { get; set; }
}


public class LatePermitForm : LatePermitListItem
{
    public Guid? ApprovalTransactionKey { get; set; } = Guid.Empty;
    public List<SelectListItem> Employees { get; set; } = new List<SelectListItem>();
    public ApprovalTransaction? ApprovalTransaction { get; set; }
    public IEnumerable<ApprovalStatusItemList> ApprovalStatuses { get; set; } = Enumerable.Empty<ApprovalStatusItemList>();

    public string JsonApprovalStatuses { get; set; } = string.Empty;

    public LatePermitDto ConvertToLatePermitDto()
    {
        return new LatePermitDto
        {
            Key = this.Key,
            EmployeeKey = this.EmployeeKey,
            DateSubmission = this.DateSubmission,
            TimeIn = this.TimeIn,
            Description = this.Description ?? String.Empty,
            ApprovalStatus = this.ApprovalStatus,
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

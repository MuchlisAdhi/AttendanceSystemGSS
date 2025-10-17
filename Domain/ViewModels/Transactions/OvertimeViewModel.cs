using Domain.Dtos.Transactions;
using Domain.Entities.Masters;
using Domain.Entities.Transactions;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Domain.ViewModels.Transactions;

public class OvertimeListItem
{
    public Guid Key { get; set; }
    public Guid? EmployeeKey { get; set; } = Guid.Empty;
    public DateOnly DateSubmission { get; set; }
    public TimeOnly TimeIn { get; set; }
    public TimeOnly TimeOut { get; set; }
    public TimeOnly OvertimeIn { get; set; }
    public TimeOnly OvertimeOut { get; set; }
    public string? Description { get; set; } = String.Empty;
    public ApprovalStatus? ApprovalStatus { get; set; }
    public string? StatusName { get; set; }
    public string Number { get; set; } = null!;
    public Employee? Employee { get; set; }
}

public class OvertimeLetterForm : OvertimeListItem
{
    public Guid? ApprovalTransactionKey { get; set; } = Guid.Empty;
    public List<SelectListItem> Employees { get; set; } = new List<SelectListItem>();
    public ApprovalTransaction? ApprovalTransaction { get; set; }
    public IEnumerable<ApprovalStatusItemList> ApprovalStatuses { get; set; } = Enumerable.Empty<ApprovalStatusItemList>();

    public string JsonApprovalStatuses { get; set; } = string.Empty;

    public OvertimeDto ConvertToOvertimeLetterDto()
    {
        return new OvertimeDto
        {
            Key = this.Key,
            EmployeeKey = this.EmployeeKey,
            ApprovalTransactionKey = this.ApprovalTransactionKey,
            DateSubmission = this.DateSubmission,
            OvertimeIn = this.OvertimeIn,
            OvertimeOut = this.OvertimeOut,
            Description = this.Description ?? String.Empty,
            ApprovalStatus = this.ApprovalStatus,
            Number = this.Number,
            TimeIn = this.TimeIn,
            TimeOut = this.TimeOut,
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
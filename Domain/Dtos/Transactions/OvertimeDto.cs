using Domain.Entities.Transactions;
using Domain.Enums;

namespace Domain.Dtos.Transactions;
public class OvertimeDto
{
    public Guid? Key { get; set; } = Guid.Empty;
    public Guid? EmployeeKey { get; set; } = Guid.Empty;
    public DateOnly? DateSubmission { get; set; }
    public TimeOnly OvertimeIn { get; set; }
    public TimeOnly OvertimeOut { get; set; }
    public string? Description { get; set; } = String.Empty;
    public ApprovalStatus? ApprovalStatus { get; set; } = Enums.ApprovalStatus.New;
    public string Number { get; set; } = null!;
    public Guid? ApprovalTransactionKey { get; set; }
    public TimeOnly? TimeIn { get; set; } = TimeOnly.MinValue;
    public TimeOnly? TimeOut { get; set; } = TimeOnly.MinValue;
    public IEnumerable<ApprovalStatusDto>? ApprovalStatuses { get; set; } = Enumerable.Empty<ApprovalStatusDto>();

    public Overtime ConvertToEntity()
    {
        return new Overtime
        {
            Key = this.Key ?? Guid.NewGuid(),
            EmployeeKey = this.EmployeeKey ?? Guid.Empty,
            DateSubmission = this.DateSubmission ?? DateOnly.FromDateTime(DateTime.Now.ToUniversalTime()),
            OvertimeIn = this.OvertimeIn,
            OvertimeOut = this.OvertimeOut,
            Description = this.Description,
            ApprovalStatus = this.ApprovalStatus ?? Enums.ApprovalStatus.New,
            Number = this.Number,
            ApprovalTransactionKey = this.ApprovalTransactionKey ?? Guid.Empty,
            TimeIn = this.TimeIn,
            TimeOut = this.TimeOut,
        };
    }
}

using Domain.Entities.Transactions;
using Domain.Enums;

namespace Domain.Dtos.Transactions;

public class OutPermitDto
{
    public Guid? Key { get; set; } = Guid.Empty;
    public Guid? EmployeeKey { get; set; } = Guid.Empty;
    public DateOnly? DateSubmission { get; set; }
    public TimeOnly? TimeOut { get; set; }
    public TimeOnly? BackToWork { get; set; }
    public string? Description { get; set; } = String.Empty;
    public ApprovalStatus? ApprovalStatus { get; set; } = Enums.ApprovalStatus.New;
    public string Number { get; set; } = null!;
    public Guid? ApprovalTransactionKey { get; set; }
    public IEnumerable<ApprovalStatusDto>? ApprovalStatuses { get; set; } = Enumerable.Empty<ApprovalStatusDto>();

    public OutPermit ConvertToEntity()
    {
        return new OutPermit
        {
            Key = this.Key ?? Guid.NewGuid(),
            EmployeeKey = this.EmployeeKey ?? Guid.Empty,
            DateSubmission = this.DateSubmission ?? DateOnly.FromDateTime(DateTime.Now.ToUniversalTime()),
            TimeOut = this.TimeOut ?? TimeOnly.FromDateTime(DateTime.Now.ToUniversalTime()),
            BackToWork = this.BackToWork ?? TimeOnly.FromDateTime(DateTime.Now.ToUniversalTime()),
            Description = this.Description ?? String.Empty,
            ApprovalStatus = this.ApprovalStatus ?? Enums.ApprovalStatus.New,
            Number = this.Number,
            ApprovalTransactionKey = this.ApprovalTransactionKey ?? Guid.Empty
        };
    }
}

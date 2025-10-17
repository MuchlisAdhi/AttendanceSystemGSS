using Domain.Enums;

namespace Domain.Dtos.Transactions;

public class ApprovalStatusDto
{
    public string Action { get; set; } = null!;
    public ApprovalStatus? Status { get; set; } = ApprovalStatus.Waiting;
    public Guid? ApproverKey { get; set; }
    public int Level { get; set; }
    public string Email { get; set; } = null!;
}

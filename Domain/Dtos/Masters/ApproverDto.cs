using Domain.Entities.Masters;
using Domain.Enums;
using Domain.ViewModels.Masters;

namespace Domain.Dtos.Masters;

public class ApproverDto
{
    public Guid? Key { get; set; } = Guid.Empty;
    public Guid ApprovalConfigKey { get; set; }
    public Guid EmployeeKey { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public ApproverCategory ApproverCategory { get; set; }
    public int Level { get; set; }
    public string Action { get; set; } = null!;

    public Approver ConvertToEntity()
    {
        return new Approver
        {
            Key = this.Key ?? Guid.Empty,
            ApprovalConfigKey = this.ApprovalConfigKey,
            EmployeeKey = this.EmployeeKey,
            Email = this.Email,
            Position = this.Position,
            ApproverCategory = this.ApproverCategory,
            Level = this.Level,
            Action = this.Action
        };
    }

    public ApproverListItem ConvertToListItem()
    {
        return new ApproverListItem
        {
            ApprovalConfigKey = this.ApprovalConfigKey,
            EmployeeKey = this.EmployeeKey,
            Email = this.Email,
            Position = this.Position,
            ApproverCategory = this.ApproverCategory,
            Level = this.Level,
            Action = this.Action
        };
    }
}

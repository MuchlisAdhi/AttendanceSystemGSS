using Domain.Dtos.Masters;
using Domain.Entities.Masters;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Domain.ViewModels.Masters;

public class ApproverListItem
{
    public Guid ApprovalConfigKey { get; set; }
    public Guid EmployeeKey { get; set; }
    public string Email { get; set; } = null!;
    public string Position { get; set; } = null!;
    public int Level { get; set; }
    public string Action { get; set; } = null!;
    public List<SelectListItem> Users { get; set; } = new List<SelectListItem>();
    public Employee? User { get; set; }
    public ApproverCategory ApproverCategory { get; set; } = ApproverCategory.DirectSupervisor;
    public bool CanDelete { get; set; } = false;

    public ApproverDto ConvertToApproverDto()
    {
        return new ApproverDto
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

public class ApproverUserList
{
    public Guid Key { get; set; }
    public string? NIK { get; set; }
    public string? Name { get; set; }
    public string? Position { get; set; }
}

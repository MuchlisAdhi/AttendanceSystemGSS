using Domain.Dtos.Masters;
using Domain.Enums;

namespace Domain.ViewModels.Masters;

public class ApprovalConfigListItem
{
    public Guid Key { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; } = String.Empty;
}

public class ApprovalConfigForm
{
    public Guid Key { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; } = String.Empty;
    public IEnumerable<ApproverListItem> Approvers { get; set; } = Enumerable.Empty<ApproverListItem>();

    public ApprovalConfigForm()
    {
        var defaultApprover = new ApproverListItem
        {
            EmployeeKey = Guid.Empty,
            ApproverCategory = ApproverCategory.DirectSupervisor,
            Level = 1,
            Action = "Approver",
            Email = string.Empty,
            Position = string.Empty,
            CanDelete = false
        };

        Approvers = new List<ApproverListItem> { defaultApprover };
    }

    public void FromDto(ApprovalConfigDto dto)
    {
        this.Name = dto.Name;
        this.Description = dto.Description;
        var approverList = new List<ApproverListItem>();

        // Always ensure Direct Supervisor is present
        var defaultApprover = new ApproverListItem
        {
            EmployeeKey = Guid.Empty,
            ApproverCategory = ApproverCategory.DirectSupervisor,
            Level = 1,
            Action = "Approver",
            Email = string.Empty,
            Position = string.Empty,
            CanDelete = false
        };
        approverList.Add(defaultApprover);
        // Add other Approvers from DTO
        if (dto.Approvers != null && dto.Approvers.Any())
        {
            approverList.AddRange(dto.Approvers.Where(x => x.ApproverCategory != ApproverCategory.DirectSupervisor)
                                              .Select(x => x.ConvertToListItem()));
        }

        this.Approvers = approverList;
    }

    public ApprovalConfigDto ConvertToApprovalConfigDto()
    {
        return new ApprovalConfigDto
        {
            Key = this.Key,
            Name = this.Name,
            Description = this.Description ?? String.Empty,
            Approvers = this.Approvers != null && this.Approvers.Any()
                        ? this.Approvers.Select(x => x.ConvertToApproverDto()).ToList()
                        : new List<ApproverDto>()
        };
    }
}

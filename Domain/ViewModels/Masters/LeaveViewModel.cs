using Domain.Dtos.Masters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Domain.ViewModels.Masters;

public class LeaveListItem
{
    public Guid Key { get; set; }
    public string? Code { get; set; } = String.Empty;
    public string? Name { get; set; } = String.Empty;
    public int? MaxDays { get; set; } = 0;
    public int? MinSubmission { get; set; } = 0;
    public int? MaxSubmission { get; set; } = 0;
    public string? Description { get; set; } = String.Empty;
}

public class LeaveList
{
    public IEnumerable<LeaveListItem> Leaves { get; set; } = new List<LeaveListItem>();
}

public class LeaveForm : LeaveListItem
{
    public List<SelectListItem> Companies { get; set; } = new List<SelectListItem>();
    public string? Description { get; set; } = String.Empty;

    public LeaveDto ConvertToLeaveDto()
    {
        return new LeaveDto
        {
            Key = this.Key,
            Code = this.Code,
            Name = this.Name,
            MaxDays = this.MaxDays,
            MinSubmission = this.MinSubmission,
            MaxSubmission = this.MaxSubmission,
            Description = this.Description
        };
    }

    public void FromDto(LeaveDto dto)
    {
        this.Code = dto.Code;
        this.Name = dto.Name;
        this.MaxDays = dto.MaxDays;
        this.MinSubmission = dto.MinSubmission;
        this.MaxSubmission = dto.MaxSubmission;
        this.Description = dto.Description;
    }
}

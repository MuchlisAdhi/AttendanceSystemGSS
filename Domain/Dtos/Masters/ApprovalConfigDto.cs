using Domain.Entities.Masters;

namespace Domain.Dtos.Masters;

public class ApprovalConfigDto
{
    public Guid? Key { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<ApproverDto> Approvers { get; set; } = [];

    public ApprovalConfig ConvertToEntity()
    {
        return new ApprovalConfig
        {
            Key = this.Key ?? Guid.Empty,
            Name = this.Name,
            Description = this.Description ?? String.Empty,
            Approvers = this.Approvers.Select(a => a.ConvertToEntity()).ToList()
        };
    }
}

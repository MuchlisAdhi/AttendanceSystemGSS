using Domain.Abstracts;
using Domain.ViewModels.Masters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Masters;

[Table("tbmapprovalconfig")]
public class ApprovalConfig : AuditTrail
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(200)]
    public string? Description { get; set; }

    [NotMapped]
    public IEnumerable<Approver>? Approvers { get; set; } = Enumerable.Empty<Approver>();

    public ApprovalConfigListItem ConvertToListItem()
    {
        return new ApprovalConfigListItem
        {
            Key = this.Key,
            Name = this.Name,
            Description = this.Description
        };
    }

    public ApprovalConfigForm ConvertToForm()
    {
        return new ApprovalConfigForm
        {
            Key = this.Key,
            Name = this.Name,
            Description = this.Description,
            Approvers = this.Approvers?.Select(x => x.ConvertToApproverListItem())
        };
    }
}

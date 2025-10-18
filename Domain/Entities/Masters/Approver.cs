using Domain.Abstracts;
using Domain.Enums;
using Domain.ViewModels.Masters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Masters;

[Table("tbmapprover")]
public class Approver : AuditTrail
{
    [Required]
    public Guid ApprovalConfigKey { get; set; }
    [Required]
    public Guid EmployeeKey { get; set; }
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = null!;
    [Required]
    [MaxLength(100)]
    public string Position { get; set; } = null!;

    [Required]
    public ApproverCategory ApproverCategory { get; set; }
    [Required]
    public int Level { get; set; }
    [Required]
    [MaxLength(20)]
    public string Action { get; set; } = null!;

    [NotMapped]
    public Employee? User { get; set; }
    [NotMapped]
    public ApprovalConfig? ApprovalConfig { get; set; }

    public ApproverListItem ConvertToApproverListItem()
    {
        return new ApproverListItem
        {
            ApprovalConfigKey = this.ApprovalConfigKey,
            EmployeeKey = this.EmployeeKey,
            Email = this.Email,
            Position = this.Position,
            Level = this.Level,
            Action = this.Action,
            User = this.User,
            ApproverCategory = this.ApproverCategory,
            CanDelete = this.ApproverCategory != ApproverCategory.DirectSupervisor
        };
    }
}

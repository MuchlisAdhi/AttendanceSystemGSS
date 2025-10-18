using Domain.Abstracts;
using Domain.ViewModels.Masters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Masters;

[Table("tbmleave")]
public class Leave : AuditTrail
{
    [Required]
    [StringLength(10)]
    public string Code { get; set; } = null!;
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
    [Required]
    public int MaxDays { get; set; }
    [Required]
    public int MinSubmission { get; set; }
    [Required]
    public int MaxSubmission { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; } = string.Empty;

    public LeaveForm ConvertToViewModelLeave()
    {
        return new LeaveForm
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
}

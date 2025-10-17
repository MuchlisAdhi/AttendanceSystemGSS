using Domain.Abstracts;
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
}

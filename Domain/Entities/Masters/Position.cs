using Domain.Abstracts;
using Domain.ViewModels.Masters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Masters;

[Table("tbmposition")]
public class Position : AuditTrail
{
    [Required]
    [StringLength(20)]
    public string Code { get; set; } = null!;
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;
    [StringLength(200)]
    public string? Description { get; set; } = string.Empty;

    public PositionForm ConvertToViewModelPositionForm()
    {
        return new PositionForm
        {
            Key = this.Key,
            Code = this.Code,
            Name = this.Name,
            Description = this.Description
        };
    }
}

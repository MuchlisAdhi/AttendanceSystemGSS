using Domain.Abstracts;
using Domain.ViewModels.Masters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Masters;

[Table("tbmgrade")]
public class Grade : AuditTrail
{
    [Required]
    [StringLength(20)]
    public string Code { get; set; } = null!;
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;
    [StringLength(200)]
    public string? Description { get; set; } = string.Empty;

    public GradeForm ConvertToViewModelForm()
    {
        return new GradeForm
        {
            Key = this.Key,
            Code = this.Code,
            Name = this.Name,
            Description = this.Description
        };
    }
}

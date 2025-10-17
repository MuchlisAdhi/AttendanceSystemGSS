using Domain.Abstracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Masters;

[Table("tbmprovince")]
public class Province : AuditTrail
{
    [Required]
    [StringLength(20)]
    public string Code { get; set; } = null!;
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
    [StringLength(200)]
    public string Description { get; set; } = string.Empty;
    [Required]
    public Guid CountryKey { get; set; }

    [NotMapped]
    public Country? Country { get; set; }
    [NotMapped]
    public IEnumerable<City>? Cities { get; set; } = Enumerable.Empty<City>();
}

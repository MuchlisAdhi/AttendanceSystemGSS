using Domain.Abstracts;
using Domain.ViewModels.Masters;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    public ProvinceForm ConvertToViewModelProvinceForm()
    {
        return new ProvinceForm()
        {
            Key = this.Key,
            Code = this.Code,
            Name = this.Name,
            Description = this.Description,
            CountryKey = this.CountryKey,
            Country = this.Country,
            Countries = new List<SelectListItem>(),
            CreatedAt = this.CreatedAt,
            CreatedBy = this.CreatedBy,
            UpdatedAt = this.UpdatedAt,
            UpdatedBy = this.UpdatedBy,
            DeletedAt = this.DeletedAt,
            DeletedBy = this.DeletedBy
        };
    }

    public ProvinceListItem ConvertToViewModelProvinceListItem()
    {
        return new ProvinceListItem()
        {
            Key = this.Key,
            Code = this.Code,
            Name = this.Name,
            Description = this.Description,
            CountryKey = this.CountryKey,
            Country = this.Country,
            CreatedAt = this.CreatedAt,
            CreatedBy = this.CreatedBy
        };
    }
}

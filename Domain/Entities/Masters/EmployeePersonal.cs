using Domain.Abstracts;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Masters;

[Table("tbmemployeepersonal")]
public class EmployeePersonal : AuditTrail
{
    [Required]
    public Guid EmployeeKey { get; set; }
    [Required]
    [StringLength(50)]
    public string PlaceOfBirth { get; set; } = null!;
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public Gender Gender { get; set; }
    [Required]
    public Religion Religion { get; set; }
    [Required]
    public MaritalStatus MaritalStatus { get; set; }
    [Required]
    [StringLength(200)]
    public string Address { get; set; } = null!;
    [Required]
    public Guid CountryKey { get; set; }
    [Required]
    public Guid ProvinceKey { get; set; }
    [Required]
    public Guid CityKey { get; set; }
    [Required]
    [StringLength(5)]
    public string PostalCode { get; set; } = null!;
    [Required]
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [NotMapped]
    public Employee? Employee { get; set; }
    [NotMapped]
    public Country? Country { get; set; }
    [NotMapped]
    public Province? Province { get; set; }
    [NotMapped]
    public City? City { get; set; }
}

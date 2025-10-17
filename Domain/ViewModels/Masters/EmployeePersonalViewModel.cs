using Domain.Dtos.Masters;
using Domain.Entities.Masters;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Domain.ViewModels.Masters;

public class EmployeePersonalList
{
    public IEnumerable<EmployeePersonal> EmployeePersonals { get; set; } = Enumerable.Empty<EmployeePersonal>();
}

public class EmployeePersonalForm
{
    public Guid? Key { get; set; } = Guid.Empty;
    public Guid? EmployeeKey { get; set; } = Guid.Empty;
    public string? PlaceOfBirth { get; set; } = String.Empty;
    public DateTime? DateOfBirth { get; set; } = DateTime.Now;
    public Gender? Gender { get; set; } = null;
    public Religion? Religion { get; set; } = null;
    public MaritalStatus? MaritalStatus { get; set; } = null;
    public string? Address { get; set; } = String.Empty;
    public Guid? CountryKey { get; set; } = Guid.Empty;
    public Guid? ProvinceKey { get; set; } = Guid.Empty;
    public Guid? CityKey { get; set; } = Guid.Empty;
    public string? PostalCode { get; set; } = String.Empty;
    public string? PhoneNumber { get; set; } = String.Empty;
    public string? Email { get; set; } = String.Empty;

    public Employee? Employee { get; set; }
    public List<SelectListItem> Religions { get; set; } = new List<SelectListItem>();
    public Country? Country { get; set; }
    public List<SelectListItem> Countries { get; set; } = new List<SelectListItem>();
    public Province? Province { get; set; }
    public List<SelectListItem> Provinces { get; set; } = new List<SelectListItem>();
    public City? City { get; set; }
    public List<SelectListItem> Cities { get; set; } = new List<SelectListItem>();

    public EmployeePersonalDto ConvertToDto()
    {
        return new EmployeePersonalDto
        {
            Key = this.Key ?? Guid.Empty,
            EmployeeKey = this.EmployeeKey ?? Guid.Empty,
            PlaceOfBirth = this.PlaceOfBirth ?? String.Empty,
            DateOfBirth = this.DateOfBirth ?? DateTime.Now,
            Gender = this.Gender ?? null,
            Religion = this.Religion ?? null,
            MaritalStatus = this.MaritalStatus ?? null,
            Address = this.Address ?? String.Empty,
            CountryKey = this.CountryKey ?? Guid.Empty,
            ProvinceKey = this.ProvinceKey ?? Guid.Empty,
            CityKey = this.CityKey ?? Guid.Empty,
            PostalCode = this.PostalCode ?? String.Empty,
            PhoneNumber = this.PhoneNumber ?? String.Empty,
            Email = this.Email ?? String.Empty
        };
    }
}

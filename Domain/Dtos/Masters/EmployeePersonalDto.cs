using Domain.Entities.Masters;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Masters;

public class EmployeePersonalDto
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
    public string? SocialMedia { get; set; } = String.Empty;

    public EmployeePersonal ConvertToEntity()
    {
        return new EmployeePersonal
        {
            Key = this.Key ?? Guid.Empty,
            EmployeeKey = this.EmployeeKey ?? Guid.Empty,
            PlaceOfBirth = this.PlaceOfBirth ?? String.Empty,
            DateOfBirth = this.DateOfBirth ?? DateTime.Now,
            Gender = this.Gender ?? Enums.Gender.Male,
            Religion = this.Religion ?? Enums.Religion.Islam,
            MaritalStatus = this.MaritalStatus ?? Enums.MaritalStatus.Single,
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

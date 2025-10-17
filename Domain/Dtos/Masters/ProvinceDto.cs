using Domain.Entities.Masters;

namespace Domain.Dtos.Masters;

public class ProvinceDto
{
    public Guid? Key { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; } = String.Empty;
    public Guid CountryKey { get; set; }

    public Province ConvertToEntity()
    {
        return new Province
        {
            Key = this.Key ?? Guid.Empty,
            Code = this.Code,
            Name = this.Name,
            Description = this.Description ?? String.Empty,
            CountryKey = this.CountryKey
        };
    }
}

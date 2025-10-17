using Domain.Entities.Masters;

namespace Domain.Dtos.Masters;

public class CountryDto
{
    public Guid? Key { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public Country ConvertToEntity()
    {
        return new Country
        {
            Key = this.Key ?? Guid.Empty,
            Code = this.Code,
            Name = this.Name,
            Description = this.Description
        };
    }
}

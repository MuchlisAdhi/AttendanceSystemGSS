using Domain.Dtos.Masters;
using Domain.Entities.Masters;

namespace Domain.ViewModels.Masters;

public class PositionList
{
    public IEnumerable<Position> Positions { get; set; } = Enumerable.Empty<Position>();
}

public class PositionForm
{
    public Guid Key { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid? CompanyKey { get; set; }

    public PositionDto ConvertToDto()
    {
        return new PositionDto
        {
            Key = this.Key,
            Code = this.Code ?? String.Empty,
            Name = this.Name ?? String.Empty,
            Description = this.Description ?? String.Empty
        };
    }
}

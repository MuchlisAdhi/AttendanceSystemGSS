using Domain.Entities.Masters;

namespace Domain.Dtos.Masters;

public class TitleDto
{
    public Guid? Key { get; set; }
    public string Code { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public string? Description { get; set; } = String.Empty;

    public Title ConverToEntity()
    {
        return new Title
        {
            Key = this.Key ?? Guid.Empty,
            Code = this.Code,
            Name = this.Name,
            Description = this.Description
        };
    }
}

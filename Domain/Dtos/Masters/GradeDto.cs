using Domain.Entities.Masters;

namespace Domain.Dtos.Masters;

public class GradeDto
{
    public Guid? Key { get; set; }
    public string Code { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;

    public Grade ConvertToEntity()
    {
        return new Grade
        {
            Key = this.Key ?? Guid.Empty,
            Code = this.Code,
            Name = this.Name,
            Description = this.Description
        };
    }
}

using Domain.Dtos.Masters;
using Domain.Entities.Masters;

namespace Domain.ViewModels.Masters;

public class GradeList
{
    public IEnumerable<Grade> Grades { get; set; } = Enumerable.Empty<Grade>();
}

public class GradeForm
{
    public Guid Key { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public GradeDto ConvertToGradeDto()
    {
        return new GradeDto
        {
            Key = this.Key,
            Code = this.Code ?? String.Empty,
            Name = this.Name ?? String.Empty,
            Description = this.Description ?? String.Empty
        };
    }
}

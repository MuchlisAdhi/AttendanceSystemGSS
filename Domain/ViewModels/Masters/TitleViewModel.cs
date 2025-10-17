using Domain.Dtos.Masters;
using Domain.Entities.Masters;

namespace Domain.ViewModels.Masters;

public class TitleList
{
    public IEnumerable<Title> Titles { get; set; } = Enumerable.Empty<Title>();
}

public class TitleForm
{
    public Guid Key { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid? CompanyKey { get; set; }

    public TitleDto ConvertToDto()
    {
        return new TitleDto
        {
            Key = this.Key,
            Code = this.Code ?? String.Empty,
            Name = this.Name ?? String.Empty,
            Description = this.Description ?? String.Empty
        };
    }
}

using Domain.Entities.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Masters;

public class LeaveDto
{
    public Guid? Key { get; set; } = Guid.Empty;
    public string? Code { get; set; } = String.Empty;
    public string? Name { get; set; } = String.Empty;
    public int? MaxDays { get; set; } = 0;
    public int? MinSubmission { get; set; } = 0;
    public int? MaxSubmission { get; set; } = 0;
    public string? Description { get; set; } = String.Empty;

    public Leave ConvertToEntity()
    {
        return new Leave
        {
            Key = this.Key ?? Guid.Empty,
            Code = this.Code ?? String.Empty,
            Name = this.Name ?? String.Empty,
            MaxDays = this.MaxDays ?? 0,
            MinSubmission = this.MinSubmission ?? 0,
            MaxSubmission = this.MaxSubmission ?? 0,
            Description = this.Description ?? String.Empty
        };
    }
}

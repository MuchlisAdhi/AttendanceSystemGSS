using Domain.Entities.Masters;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Masters;

public class EmployeeDto
{
    public Guid? Key { get; set; } = Guid.Empty;
    public string? Code { get; set; } = String.Empty;
    public string Name { get; set; } = null!;
    public Guid? PositionKey { get; set; } = Guid.Empty;
    public Guid? TitleKey { get; set; } = Guid.Empty;
    public Guid? GradeKey { get; set; } = Guid.Empty;
    public DateTime? HireDate { get; set; } = DateTime.Now;
    public EmployeeStatus? Status { get; set; } = null;
    public Guid? DirectSupervisorKey { get; set; } = Guid.Empty;
    public DateTime? ResignDate { get; set; } = null;

    public EmployeePersonalDto? EmployeePersonal { get; set; } = null;

    public Employee ConvertToEntity()
    {
        return new Employee
        {
            Key = this.Key ?? Guid.Empty,
            Code = this.Code ?? String.Empty,
            Name = this.Name,
            PositionKey = this.PositionKey ?? Guid.Empty,
            TitleKey = this.TitleKey ?? Guid.Empty,
            GradeKey = this.GradeKey ?? Guid.Empty,
            HireDate = this.HireDate ?? DateTime.Now,
            Status = this.Status ?? EmployeeStatus.Contract,
            DirectSupervisorKey = this.DirectSupervisorKey ?? Guid.Empty,
            EmployeePersonal = this.EmployeePersonal?.ConvertToEntity()
        };
    }
}

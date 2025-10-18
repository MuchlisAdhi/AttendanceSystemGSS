using Domain.Dtos.Masters;
using Domain.Entities.Masters;
using Domain.Enums;
using Domain.ViewModels.Transactions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Domain.ViewModels.Masters;

public class EmployeeList
{
    public IEnumerable<Employee> Employees { get; set; } = Enumerable.Empty<Employee>();
}

public class EmployeeItemPagination
{
    public Employee Employee { get; set; } = null!;
    public Position Position { get; set; } = null!;
    public Title Title { get; set; } = null!;
    public Grade Grade { get; set; } = null!;
    public Employee? DirectSupervisor { get; set; } = null;
}

public class DirectSupervisorList
{
    public Guid Key { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
}

public class EmployeeForm
{
    public Guid Key { get; set; }
    public string? Code { get; set; } = string.Empty;
    public string Name { get; set; } = null!;
    public Guid? PositionKey { get; set; } = Guid.Empty;
    public Guid? TitleKey { get; set; } = Guid.Empty;
    public Guid? GradeKey { get; set; } = Guid.Empty;
    public DateTime? HireDate { get; set; } = DateTime.Now;
    public EmployeeStatus? Status { get; set; } = null;
    public Guid? DirectSupervisorKey { get; set; } = Guid.Empty;
    public Position? Position { get; set; }
    public List<SelectListItem> Positions { get; set; } = new List<SelectListItem>();
    public Title? Title { get; set; }
    public List<SelectListItem> Titles { get; set; } = new List<SelectListItem>();
    public Grade? Grade { get; set; }
    public List<SelectListItem> Grades { get; set; } = new List<SelectListItem>();
    public Employee? DirectSupervisor { get; set; }
    public List<SelectListItem> DirectSupervisors { get; set; } = new List<SelectListItem>();
    public EmployeePersonalForm? EmployeePersonal { get; set; }
    public EmployeeAttendanceForm? EmployeeAttendance { get; set; }

    public EmployeeDto ConvertToDto()
    {
        return new EmployeeDto
        {
            Key = this.Key,
            Code = this.Code ?? String.Empty,
            Name = this.Name,
            PositionKey = this.PositionKey ?? Guid.Empty,
            TitleKey = this.TitleKey ?? Guid.Empty,
            GradeKey = this.GradeKey ?? Guid.Empty,
            HireDate = this.HireDate ?? DateTime.Now,
            Status = this.Status ?? null,
            DirectSupervisorKey = this.DirectSupervisorKey ?? Guid.Empty,
            EmployeePersonal = this.EmployeePersonal?.ConvertToDto() ?? null
        };
    }
}

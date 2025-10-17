using Domain.Dtos.Transactions;
using Domain.Entities.Masters;
using Domain.ViewModels.Masters;

namespace Domain.ViewModels.Transactions;

public class EmployeeAttendanceForm
{
    public Guid? Key { get; set; } = Guid.Empty;
    public Guid? EmployeeKey { get; set; } = Guid.Empty;
    public Guid? ApprovalConfigKey { get; set; } = Guid.Empty;
    public Employee? Employee { get; set; }
    public ApprovalConfigForm? ApprovalConfig { get; set; }

    //For Deserialization or Serialization Input form array
    public string JsonEmployeeAttendanceDetails { get; set; } = String.Empty;

    public EmployeeAttendanceDto ConvertToEmployeeAttendanceDto()
    {
        return new EmployeeAttendanceDto
        {
            Key = this.Key ?? Guid.Empty,
            EmployeeKey = this.EmployeeKey ?? Guid.Empty,
            ApprovalConfigKey = this.ApprovalConfigKey ?? Guid.Empty
        };
    }
}

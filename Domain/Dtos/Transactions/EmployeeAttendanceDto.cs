using Domain.Entities.Transactions;

namespace Domain.Dtos.Transactions;

public class EmployeeAttendanceDto
{
    public Guid? Key { get; set; } = Guid.Empty;
    public Guid EmployeeKey { get; set; }
    public Guid ApprovalConfigKey { get; set; }

    public EmployeeAttendance ConvertToEntity()
    {
        return new EmployeeAttendance
        {
            Key = this.Key ?? Guid.Empty,
            EmployeeKey = this.EmployeeKey,
            ApprovalConfigKey = this.ApprovalConfigKey
        };
    }
}

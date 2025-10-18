using Domain.Abstracts;
using Domain.Entities.Masters;
using Domain.ViewModels.Transactions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Transactions;

[Table("tbtemployeeattendance")]
public class EmployeeAttendance : AuditTrail
{
    [Required]
    public Guid EmployeeKey { get; set; }
    [Required]
    public Guid ApprovalConfigKey { get; set; }
    [NotMapped]
    public Employee? Employee { get; set; }
    [NotMapped]
    public ApprovalConfig? ApprovalConfig { get; set; }

    public EmployeeAttendanceForm ConvertToEmployeeAttendanceForm()
    {
        return new EmployeeAttendanceForm
        {
            Key = this.Key,
            EmployeeKey = this.EmployeeKey,
            Employee = this.Employee,
            ApprovalConfigKey = this.ApprovalConfigKey,
            ApprovalConfig = this.ApprovalConfig?.ConvertToForm()
        };
    }
}

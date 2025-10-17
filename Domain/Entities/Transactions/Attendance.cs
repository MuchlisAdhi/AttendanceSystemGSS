using Domain.Abstracts;
using Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Transactions;

[Table("tbtattendance")]
public class Attendance : AuditTrail
{
    [Required]
    public Guid EmployeeKey { get; set; }
    [Required]
    public DateOnly AttendanceDate { get; set; }
    [Required]
    public TimeOnly TimeIn { get; set; }
    [Required]
    public TimeOnly TimeOut { get; set; }
    [Required]
    public Double Latitude { get; set; }
    [Required]
    public Double Longitude { get; set; }
    public Guid? EarlyOutKey { get; set; } = Guid.Empty;
    public Guid? LatePermitKey { get; set; } = Guid.Empty;
    public Guid? OvertimeKey { get; set; } = Guid.Empty;
    public Guid? OutPermitKey { get; set; } = Guid.Empty;
    public Guid? LeaveSubmissionKey { get; set; } = Guid.Empty;

    [NotMapped]
    public Employee Employee { get; set; } = null!;
    [NotMapped]
    public EarlyOut? EarlyOut { get; set; }
    [NotMapped]
    public LatePermit? LatePermit { get; set; }
    [NotMapped]
    public Overtime? Overtime { get; set; }
    [NotMapped]
    public OutPermit? OutPermit { get; set; }
    [NotMapped]
    public LeaveSubmission? LeaveSubmission { get; set; }
}

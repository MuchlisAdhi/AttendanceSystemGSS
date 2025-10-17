using Domain.Abstracts;
using Domain.Entities.Masters;
using Domain.Enums;
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
    public AttendanceStatus Status { get; set; }
    public double? CheckInLatitude { get; set; }
    public double? CheckInLongitude { get; set; }
    public double? CheckOutLatitude { get; set; }
    public double? CheckOutLongitude { get; set; }
    public Guid? EarlyOutKey { get; set; } = Guid.Empty;
    public Guid? LatePermitKey { get; set; } = Guid.Empty;
    public Guid? OvertimeKey { get; set; } = Guid.Empty;
    public Guid? OutPermitKey { get; set; } = Guid.Empty;
    public Guid? LeaveSubmissionKey { get; set; } = Guid.Empty;
    [MaxLength(200)]
    public string? Description { get; set; } = String.Empty;

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

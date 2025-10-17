using Domain.Dtos.Transactions;
using Domain.Entities.Masters;
using Domain.Entities.Transactions;
using Domain.Enums;

namespace Domain.ViewModels.Transactions;

public class AttendanceForm
{
    public Guid Key { get; set; }
    public Guid? EmployeeKey { get; set; }
    public DateOnly AttendanceDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public TimeOnly? TimeIn { get; set; }
    public TimeOnly? TimeOut { get; set; }
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
    public string? Description { get; set; } = String.Empty;
    public Employee? Employee { get; set; }
    public EarlyOut? EarlyOut { get; set; }
    public LatePermit? LatePermit { get; set; }
    public Overtime? Overtime { get; set; }
    public LeaveSubmission? LeaveSubmission { get; set; }

    public AttendanceDto ConvertToAttendanceDto()
    {
        return new AttendanceDto
        {
            Key = this.Key,
            EmployeeKey = this.EmployeeKey ?? Guid.Empty,
            AttendanceDate = this.AttendanceDate,
            TimeIn = this.TimeIn,
            TimeOut = this.TimeOut,
            Status = this.Status,
            Description = this.Description,
            CheckInLatitude = this.CheckInLatitude,
            CheckInLongitude = this.CheckInLongitude,
            CheckOutLatitude = this.CheckOutLatitude,
            CheckOutLongitude = this.CheckOutLongitude,
            EarlyOutKey = this.EarlyOutKey,
            LatePermitKey = this.LatePermitKey,
            LeaveSubmissionKey = this.LeaveSubmissionKey,
            OvertimeKey = this.OvertimeKey,
        };
    }
}

public class AttendanceListItem : AttendanceForm
{
    public Position? Position { get; set; }
    public Title? Title { get; set; }
}

using Domain.Entities.Transactions;
using Domain.Enums;

namespace Domain.Dtos.Transactions;

public class AttendanceDto
{
    public Guid? Key { get; set; } = Guid.Empty;
    public Guid? EmployeeKey { get; set; } = Guid.Empty;
    public DateOnly? AttendanceDate { get; set; }
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

    public Attendance ConvertToEntity()
    {
        return new Attendance
        {
            Key = this.Key ?? Guid.Empty,
            EmployeeKey = this.EmployeeKey ?? Guid.Empty,
            AttendanceDate = this.AttendanceDate ?? DateOnly.FromDateTime(DateTime.Now),
            TimeIn = this.TimeIn ?? TimeOnly.MinValue,
            TimeOut = this.TimeOut ?? TimeOnly.MinValue,
            Status = this.Status,
            Description = this.Description ?? String.Empty,
            CheckInLatitude = this.CheckInLatitude ?? 0,
            CheckInLongitude = this.CheckInLongitude ?? 0,
            CheckOutLatitude = this.CheckOutLatitude ?? 0,
            CheckOutLongitude = this.CheckOutLongitude ?? 0,
            EarlyOutKey = this.EarlyOutKey,
            LatePermitKey = this.LatePermitKey,
            OvertimeKey = this.OvertimeKey,
            OutPermitKey = this.OutPermitKey,
            LeaveSubmissionKey = this.LeaveSubmissionKey
        };
    }
}

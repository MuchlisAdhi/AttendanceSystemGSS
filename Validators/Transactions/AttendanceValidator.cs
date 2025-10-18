using Domain.Dtos.Transactions;
using FluentValidation;

namespace Validators.Transactions;

public class AttendanceValidator : AbstractValidator<AttendanceDto>
{
    public AttendanceValidator()
    {
        RuleFor(r => r.EmployeeKey).NotNull().NotEmpty().WithMessage("Employee Key cannot be null or empty");
        RuleFor(r => r.AttendanceDate).Must(BeAValidDate).WithMessage("Attendance Date is required");
        RuleFor(r => r.TimeIn).NotNull().NotEmpty().WithMessage("TimeIn cannot be null or empty");
        RuleFor(r => r.TimeOut).NotNull().NotEmpty().WithMessage("Out cannot be null or empty");
        RuleFor(r => r.Status).IsInEnum().WithMessage("Not valid Attendance Status");
        RuleFor(x => x.Description)
        .MaximumLength(200)
        .WithMessage("Description cannot exceed 200 characters.")
        .When(x => !string.IsNullOrEmpty(x.Description));
        RuleFor(x => x.CheckInLatitude)
            .InclusiveBetween(-90.0, 90.0)
            .WithMessage("Latitude must be between -90 and 90 degrees.")
            .When(x => x.CheckInLatitude.HasValue);
        RuleFor(x => x.CheckInLongitude)
            .InclusiveBetween(-180.0, 180.0)
            .WithMessage("Longitude must be between -180 and 180 degrees.")
            .When(x => x.CheckInLongitude.HasValue);
        RuleFor(x => x.CheckOutLatitude)
            .InclusiveBetween(-90.0, 90.0)
            .WithMessage("Latitude must be between -90 and 90 degrees.")
            .When(x => x.CheckOutLatitude.HasValue);
        RuleFor(x => x.CheckInLongitude)
            .InclusiveBetween(-180.0, 180.0)
            .WithMessage("Longitude must be between -180 and 180 degrees.")
            .When(x => x.CheckOutLongitude.HasValue);
    }

    private bool BeAValidDate(DateOnly date)
    {
        if (date == default(DateOnly))
            return false;
        return true;
    }

    private bool BeAValidDate(DateOnly? date)
    {
        if (date == default(DateOnly))
            return false;
        return true;
    }
}

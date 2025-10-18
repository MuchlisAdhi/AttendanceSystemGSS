using Domain.Dtos.Masters;
using FluentValidation;

namespace Validators.Masters;

public class EmployeeValidator : AbstractValidator<EmployeeDto>
{
    public EmployeeValidator()
    {
        RuleFor(r => r.Code).NotNull().NotEmpty().WithMessage("Code cannot be null or empty")
                            .MaximumLength(30).WithMessage("Code must not exceed 30 characters");
        RuleFor(r => r.Name).NotNull().NotEmpty().WithMessage("Name cannot be null or empty")
                            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
        RuleFor(r => r.PositionKey).NotNull().NotEmpty().WithMessage("Position cannot be null or empty");
        RuleFor(r => r.TitleKey).NotNull().NotEmpty().WithMessage("Title cannot be null or empty");
        RuleFor(r => r.GradeKey).NotNull().NotEmpty().WithMessage("Grade cannot be null or empty");
        RuleFor(r => r.HireDate).Must(BeAValidDate).WithMessage("Hire Date is required");
        RuleFor(r => r.Status).IsInEnum().WithMessage("Not valid employee status");
    }

    private bool BeAValidDate(DateTime date)
    {
        if (date == default(DateTime))
            return false;
        return true;
    }

    private bool BeAValidDate(DateTime? date)
    {
        if (date == default(DateTime))
            return false;
        return true;
    }
}

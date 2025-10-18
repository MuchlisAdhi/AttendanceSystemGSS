using Domain.Dtos.Masters;
using FluentValidation;

namespace Validators.Masters;

public class ApproverValidator : AbstractValidator<ApproverDto>
{
    public ApproverValidator()
    {
        RuleFor(x => x.ApprovalConfigKey).NotNull().NotEmpty().WithMessage("Approval Config cannot be null or empty");
        RuleFor(x => x.EmployeeKey).NotNull().NotEmpty().WithMessage("Employee cannot be null or empty");
        RuleFor(r => r.Email).EmailAddress().WithMessage("Not valid email address")
                                      .MaximumLength(100).WithMessage("Email must not exceed 100 characters")
                                      .When(r => !string.IsNullOrEmpty(r.Email));
        RuleFor(r => r.Position).NotNull().NotEmpty().WithMessage("Position cannot be null or empty").MaximumLength(100).WithMessage("Position must not exceed 100 characters");
        RuleFor(r => r.Level).NotNull().WithMessage("Level must not be null")
                             .GreaterThan(0).WithMessage("Level must be greater than zero");
        RuleFor(r => r.Action).NotNull().NotEmpty().WithMessage("Action cannot be null or empty")
            .MaximumLength(20).WithMessage("Action must not exceed 20 characters");
    }
}

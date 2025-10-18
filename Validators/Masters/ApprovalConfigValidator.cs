using Domain.Dtos.Masters;
using FluentValidation;

namespace Validators.Masters;

public class ApprovalConfigValidator : AbstractValidator<ApprovalConfigDto>
{
    public ApprovalConfigValidator()
    {
        RuleFor(r => r.Name).NotNull().NotEmpty().WithMessage("Name cannot be null or empty").MaximumLength(100).WithMessage("Name must not exceed 100 characters");
        RuleFor(x => x.Description)
        .MaximumLength(200)
        .WithMessage("Description cannot exceed 200 characters.")
        .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

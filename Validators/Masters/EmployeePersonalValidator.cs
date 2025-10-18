using Domain.Dtos.Masters;
using FluentValidation;

namespace Validators.Masters;

public class EmployeePersonalValidator : AbstractValidator<EmployeePersonalDto>
{
    public EmployeePersonalValidator()
    {
        RuleSet("Create", () =>
        {
            RuleFor(r => r.PlaceOfBirth).NotNull().NotEmpty().WithMessage("Place Of Birth cannot be null or empty")
                   .MaximumLength(50).WithMessage("Place Of Birth must not exceed 50 characters");
            RuleFor(r => r.DateOfBirth).Must(BeAValidDate).WithMessage("Date of Birth is required");
            RuleFor(r => r.Gender).IsInEnum().WithMessage("Not valid gender");
            RuleFor(r => r.Religion).IsInEnum().WithMessage("Not valid religion");
            RuleFor(r => r.MaritalStatus).IsInEnum().WithMessage("Not valid marital status");
            RuleFor(r => r.Address).NotNull().NotEmpty().WithMessage("Address cannot be null or empty")
                                .MaximumLength(200).WithMessage("Address must not exceed 200 characters");
            RuleFor(r => r.CountryKey).NotNull().NotEmpty().WithMessage("Country Key cannot be null or empty");
            RuleFor(r => r.ProvinceKey).NotNull().NotEmpty().WithMessage("Province Key cannot be null or empty");
            RuleFor(r => r.CityKey).NotNull().NotEmpty().WithMessage("City Key cannot be null or empty");
            RuleFor(r => r.PostalCode).NotNull().NotEmpty().WithMessage("Postal Code cannot be null or empty")
                                    .Matches(@"^\d{5}$").WithMessage("Postal Code must contain exactly 5 digits");
            RuleFor(r => r.PhoneNumber).NotNull().NotEmpty().WithMessage("Phone Number cannot be null or empty")
                                   .Matches(@"^(?:\+62|62|0)[2-9]\d{7,11}$").WithMessage("Not Valid Phone Number")
                                   .MaximumLength(20).WithMessage("Phone Number must not exceed 20 characters");
            RuleFor(r => r.Email).NotNull().NotEmpty().WithMessage("Email cannot be null or empty")
                                 .EmailAddress().WithMessage("Not valid email address")
                                 .MaximumLength(100).WithMessage("Email must not exceed 100 characters");
            RuleFor(r => r.SocialMedia).MaximumLength(100).When(r => !string.IsNullOrEmpty(r.SocialMedia))
                                     .WithMessage("SocialMedia must not exceed 100 characters if provided.");
        });

        RuleSet("Update", () =>
        {
            RuleFor(r => r.EmployeeKey).NotNull().NotEmpty().WithMessage("Employee Key cannot be null or empty");
            RuleFor(r => r.PlaceOfBirth).NotNull().NotEmpty().WithMessage("Place Of Birth cannot be null or empty")
                   .MaximumLength(50).WithMessage("Place Of Birth must not exceed 50 characters");
            RuleFor(r => r.DateOfBirth).Must(BeAValidDate).WithMessage("Date of Birth is required");
            RuleFor(r => r.Gender).IsInEnum().WithMessage("Not valid gender");
            RuleFor(r => r.Religion).IsInEnum().WithMessage("Not valid religion");
            RuleFor(r => r.MaritalStatus).IsInEnum().WithMessage("Not valid marital status");
            RuleFor(r => r.Address).NotNull().NotEmpty().WithMessage("Address cannot be null or empty")
                                .MaximumLength(200).WithMessage("Address must not exceed 200 characters");
            RuleFor(r => r.CountryKey).NotNull().NotEmpty().WithMessage("Country Key cannot be null or empty");
            RuleFor(r => r.ProvinceKey).NotNull().NotEmpty().WithMessage("Province Key cannot be null or empty");
            RuleFor(r => r.CityKey).NotNull().NotEmpty().WithMessage("City Key cannot be null or empty");
            RuleFor(r => r.PostalCode).NotNull().NotEmpty().WithMessage("Postal Code cannot be null or empty")
                                    .Matches(@"^\d{5}$").WithMessage("Postal Code must contain exactly 5 digits");
            RuleFor(r => r.PhoneNumber).NotNull().NotEmpty().WithMessage("Phone Number cannot be null or empty")
                                   .Matches(@"^(?:\+62|62|0)[2-9]\d{7,11}$").WithMessage("Not Valid Phone Number")
                                   .MaximumLength(20).WithMessage("Phone Number must not exceed 20 characters");
            RuleFor(r => r.Email).NotNull().NotEmpty().WithMessage("Email cannot be null or empty")
                                 .EmailAddress().WithMessage("Not valid email address")
                                 .MaximumLength(100).WithMessage("Email must not exceed 100 characters");
            RuleFor(r => r.SocialMedia).MaximumLength(100).When(r => !string.IsNullOrEmpty(r.SocialMedia))
                                     .WithMessage("SocialMedia must not exceed 100 characters if provided.");
        });
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

    private bool BeAValidDateOnly(DateOnly date)
    {
        if (date == default(DateOnly))
            return false;
        return true;
    }

    private bool BeAValidDateOnly(DateOnly? date)
    {
        if (date == default(DateOnly))
            return false;
        return true;
    }
}

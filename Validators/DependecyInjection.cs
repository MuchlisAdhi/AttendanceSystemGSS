using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Validators.Masters;
using Validators.Transactions;

namespace Validators;

public static class DependecyInjection
{
    public static IServiceCollection AddValidator(this IServiceCollection services)
    {
        #region Master
        services.AddValidatorsFromAssemblyContaining<ApprovalConfigValidator>();
        services.AddValidatorsFromAssemblyContaining<ApproverValidator>();
        services.AddValidatorsFromAssemblyContaining<CountryValidator>();
        services.AddValidatorsFromAssemblyContaining<CityValidator>();
        services.AddValidatorsFromAssemblyContaining<EmployeePersonalValidator>();
        services.AddValidatorsFromAssemblyContaining<EmployeeValidator>();
        services.AddValidatorsFromAssemblyContaining<GradeValidator>();
        services.AddValidatorsFromAssemblyContaining<LeaveValidator>();
        services.AddValidatorsFromAssemblyContaining<PositionValidator>();
        services.AddValidatorsFromAssemblyContaining<ProvinceValidator>();
        services.AddValidatorsFromAssemblyContaining<TitleValidator>();
        #endregion

        #region Transaction
        services.AddValidatorsFromAssemblyContaining<ApprovalStampValidator>();
        services.AddValidatorsFromAssemblyContaining<ApprovalTransactionValidator>();
        services.AddValidatorsFromAssemblyContaining<AttendanceValidator>();
        services.AddValidatorsFromAssemblyContaining<EarlyOutValidator>();
        services.AddValidatorsFromAssemblyContaining<LatePermitValidator>();
        services.AddValidatorsFromAssemblyContaining<LeaveSubmissionValidator>();
        services.AddValidatorsFromAssemblyContaining<OutPermitValidator>();
        services.AddValidatorsFromAssemblyContaining<OvertimeValidator>();
        #endregion

        return services;
    }
}

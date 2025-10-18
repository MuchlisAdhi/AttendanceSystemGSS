using Mediator.Services;
using Microsoft.Extensions.DependencyInjection;
using Validators;

namespace Mediator;

public static class ServiceExtension
{
    public static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        services.AddValidator();
        services.AddTransient<IEmployeeRepository, EmployeeRepository>();
    }
}

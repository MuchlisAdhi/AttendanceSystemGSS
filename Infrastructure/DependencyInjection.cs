using DomainShared.Interfaces;
using DomainShared.Services;
using Infrastructure.Services;
using Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<ICurrentUser, CurrentUserService>();

        services.AddPersistence(config);
        services.AddMediator();

        return services;
    }
}

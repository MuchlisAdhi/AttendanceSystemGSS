using DomainShared.Interfaces;
using DomainShared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;

namespace Persistence;

public static class ServiceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddDbContext<DataContext>(opt =>
                    opt.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => {
                            b.MigrationsAssembly(typeof(DataContext).Assembly.FullName);
                        }
                    )
            , ServiceLifetime.Transient);

        services.AddTransient<IDataContext>(provider => provider.GetRequiredService<DataContext>());

        return services;
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PracticeLogger.Application.Common.Interfaces;
using PracticeLogger.Infrastructure.Persistence;

namespace PracticeLogger.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                "Host=localhost;Port=5432;Database=PracticeLoggerDb;Username=sa;Password=Password123!"
            )
        );

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>()
        );
        services.AddScoped<ApplicationDbContextInitialiser>();
        return services;
    }
}

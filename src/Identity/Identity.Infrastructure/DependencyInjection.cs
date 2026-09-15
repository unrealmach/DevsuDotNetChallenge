using Identity.Application.Ports.Output.Read;
using Identity.Application.Ports.Output.Security;
using Identity.Application.Ports.Output.Write;
using Identity.Infrastructure.Adapters.Persistence;
using Identity.Infrastructure.Adapters.Persistence.Read;
using Identity.Infrastructure.Adapters.Persistence.Write;
using Identity.Infrastructure.Adapters.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsql => npgsql.MigrationsHistoryTable("__ef_migrations", IdentityDbContext.Schema)));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<IdentityDbContext>());
        services.AddScoped<IUserRepository, UserRepositoryAdapter>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepositoryAdapter>();

        services.AddScoped<IUserReadRepository, UserReadRepositoryAdapter>();

        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasherAdapter>();
        services.AddSingleton<ITokenGenerator, JwtTokenGeneratorAdapter>();

        return services;
    }
}

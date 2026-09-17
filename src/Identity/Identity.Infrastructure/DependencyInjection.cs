using Identity.Application.Ports.Output.Read;
using Identity.Application.Ports.Output.Security;
using Identity.Application.Ports.Output.Write;
using Identity.Infrastructure.Adapters.Messaging;
using Identity.Infrastructure.Adapters.Persistence;
using Identity.Infrastructure.Adapters.Persistence.Read;
using Identity.Infrastructure.Adapters.Persistence.Write;
using Identity.Infrastructure.Adapters.Security;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(
        this IServiceCollection services,
        string connectionString,
        IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsql => npgsql.MigrationsHistoryTable("__ef_migrations", IdentityDbContext.Schema)));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<IdentityDbContext>());
        services.AddScoped<IClientWriteRepository, ClientWriteRepositoryAdapter>();
        services.AddScoped<IClientReadRepository, ClientReadRepositoryAdapter>();

        services.AddOptions<AesGcmOptions>()
            .Bind(configuration.GetSection(AesGcmOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<ICredentialEncryptor, AesGcmCredentialEncryptorAdapter>();

        services.AddOptions<RabbitMqOptions>()
            .Bind(configuration.GetSection(RabbitMqOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMq = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                cfg.Host(rabbitMq.Host, rabbitMq.VirtualHost, host =>
                {
                    host.Username(rabbitMq.Username);
                    host.Password(rabbitMq.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}

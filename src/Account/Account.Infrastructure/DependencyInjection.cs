using Account.Application.Ports.Output.Locking;
using Account.Application.Ports.Output.Read;
using Account.Application.Ports.Output.Write;
using Account.Infrastructure.Adapters.Locking;
using Account.Infrastructure.Adapters.Messaging;
using Account.Infrastructure.Adapters.Persistence;
using Account.Infrastructure.Adapters.Persistence.Read;
using Account.Infrastructure.Adapters.Persistence.Write;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Messaging;
using StackExchange.Redis;

namespace Account.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountInfrastructure(
        this IServiceCollection services,
        string connectionString,
        IConfiguration configuration)
    {
        services.AddDbContext<AccountDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsql => npgsql.MigrationsHistoryTable("__ef_migrations", AccountDbContext.Schema)));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AccountDbContext>());

        // repositorios de lectura
        services.AddScoped<IAccountReadRepository, AccountReadRepositoryAdapter>();
        services.AddScoped<IClientReadRepository, ClientReadRepositoryAdapter>();
        services.AddScoped<IMovementReadRepository, MovementReadRepositoryAdapter>();

        // repositorios de escritura
        services.AddScoped<IAccountWriteRepository, AccountWriteRepositoryAdapter>();
        services.AddScoped<IBalanceRepository, BalanceRepositoryAdapter>();
        services.AddScoped<IHistoryWriteRepository, HistoryWriteRepositoryAdapter>();

        services.AddOptions<RedisOptions>()
            .Bind(configuration.GetSection(RedisOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var redis = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
            return ConnectionMultiplexer.Connect(redis.ConnectionString);
        });
        services.AddScoped<IDistributedLock, RedisDistributedLock>();

        services.AddOptions<RabbitMqOptions>()
            .Bind(configuration.GetSection(RabbitMqOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ClientUpsertedIntegrationEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMq = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                cfg.Host(rabbitMq.Host, rabbitMq.VirtualHost, host =>
                {
                    host.Username(rabbitMq.Username);
                    host.Password(rabbitMq.Password);
                });

                // Sin esto, un fallo en el consumer va directo a la cola _error
                // en el primer intento. Con backoff exponencial le damos margen
                // a fallas transitorias (ej. Postgres reiniciando) antes de darlo por perdido.
                cfg.UseMessageRetry(r => r.Exponential(
                    retryLimit: 5,
                    minInterval: TimeSpan.FromSeconds(1),
                    maxInterval: TimeSpan.FromSeconds(30),
                    intervalDelta: TimeSpan.FromSeconds(2)));

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}

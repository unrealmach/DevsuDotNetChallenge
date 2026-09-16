using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Identity.Api.Extensions;

public static class ObservabilityExtensions
{
    private const string ServiceName = "Identity.Api";
    private const string OtlpEndpointConfigKey = "OpenTelemetry:OtlpEndpoint";
    private const string DefaultOtlpEndpoint = "http://localhost:18889";

    public static IServiceCollection AddIdentityObservability(this IServiceCollection services, IConfiguration configuration)
    {
        var otlpEndpoint = GetOtlpEndpoint(configuration);

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(ServiceName))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddSource("MassTransit")
                .AddOtlpExporter(options => options.Endpoint = otlpEndpoint))
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddMeter("MassTransit")
                .AddOtlpExporter(options => options.Endpoint = otlpEndpoint));

        return services;
    }

    public static ILoggingBuilder AddIdentityObservability(this ILoggingBuilder logging, IConfiguration configuration)
    {
        var otlpEndpoint = GetOtlpEndpoint(configuration);

        logging.AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            options.AddOtlpExporter(otlp => otlp.Endpoint = otlpEndpoint);
        });

        return logging;
    }

    private static Uri GetOtlpEndpoint(IConfiguration configuration) =>
        new(configuration[OtlpEndpointConfigKey] ?? DefaultOtlpEndpoint);
}

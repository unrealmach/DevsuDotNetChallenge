using Identity.Application.Ports.Output.Messaging;
using MassTransit;

namespace Identity.Infrastructure.Adapters.Messaging;

internal sealed class IntegrationEventPublisherAdapter(IPublishEndpoint publishEndpoint) : IIntegrationEventPublisher
{
    public Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken ct = default) where TEvent : class =>
        publishEndpoint.Publish(integrationEvent, ct);
}

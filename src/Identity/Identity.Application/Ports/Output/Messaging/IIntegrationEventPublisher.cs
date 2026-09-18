namespace Identity.Application.Ports.Output.Messaging;

internal interface IIntegrationEventPublisher
{
    Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken ct = default) where TEvent : class;
}

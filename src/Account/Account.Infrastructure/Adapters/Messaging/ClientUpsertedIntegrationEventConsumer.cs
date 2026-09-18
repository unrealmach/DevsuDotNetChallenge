using Account.Domain.Entities;
using Account.Infrastructure.Adapters.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.IntegrationEvents;

namespace Account.Infrastructure.Adapters.Messaging;

internal sealed class ClientUpsertedIntegrationEventConsumer(AccountDbContext context)
    : IConsumer<ClientUpsertedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ClientUpsertedIntegrationEvent> context1)
    {
        var message = context1.Message;

        var client = await context.Clients.FirstOrDefaultAsync(c => c.Id == message.Id, context1.CancellationToken);

        if (client is null)
        {
            client = Client.CreateWithId(message.Id);
            context.Clients.Add(client);
        }

        client.WithName(message.Name);
        client.WithGender(message.Gender);
        client.WithAge(message.Age);
        client.WithIdentification(message.Identification);
        client.WithAddress(message.Address);
        client.WithPhone(message.Phone);
        client.WithClientId(message.ClientId);
        client.WithStatus(message.Status);

        await context.SaveChangesAsync(context1.CancellationToken);
    }
}

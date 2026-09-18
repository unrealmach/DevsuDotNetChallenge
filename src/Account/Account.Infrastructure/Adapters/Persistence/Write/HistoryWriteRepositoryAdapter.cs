using Account.Application.Ports.Output.Write;
using Account.Domain.Entities;

namespace Account.Infrastructure.Adapters.Persistence.Write;

internal sealed class HistoryWriteRepositoryAdapter(AccountDbContext context) : IHistoryWriteRepository
{
    public async Task AddAsync(History history, CancellationToken ct = default) =>
        await context.Histories.AddAsync(history, ct);
}

using Account.Domain.Entities;

namespace Account.Application.Ports.Output.Write;

internal interface IHistoryWriteRepository
{
    Task AddAsync(History history, CancellationToken ct = default);
}

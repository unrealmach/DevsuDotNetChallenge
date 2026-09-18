using Account.Application.Dtos;
using Account.Application.Ports.Output.Read;
using Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.Adapters.Persistence.Read;

internal sealed class MovementReadRepositoryAdapter(AccountDbContext context) : IMovementReadRepository
{
    public async Task<IReadOnlyList<MovementDto>> GetAllAsync(Guid? accountId, CancellationToken ct = default) =>
        await context.Histories
            .Where(history => accountId == null || history.AccountId == accountId)
            .OrderByDescending(history => history.Date)
            .Select(ProjectToDto)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<MovementDto>> GetByRangeAsync(
        DateTime from,
        DateTime to,
        Guid? clientId,
        CancellationToken ct = default) =>
        await context.Histories
            .Where(history => history.Date >= from && history.Date <= to)
            .Where(history => clientId == null || history.ClientId == clientId)
            .OrderBy(history => history.Date)
            .Select(ProjectToDto)
            .ToListAsync(ct);

    private static readonly System.Linq.Expressions.Expression<Func<History, MovementDto>> ProjectToDto =
        history => new MovementDto(
            history.Id,
            history.AccountId,
            history.Account.Type,
            history.ClientId,
            history.Client.Name,
            history.Date,
            history.Type,
            history.Value,
            history.AvailableBalance);
}

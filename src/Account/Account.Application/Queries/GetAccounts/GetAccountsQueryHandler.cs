using Account.Application.Abstractions.Messaging;
using Account.Application.Common;
using Account.Application.Dtos;
using Account.Application.Services;

namespace Account.Application.Queries.GetAccounts;

internal sealed class GetAccountsQueryHandler(IAccountReadService accounts)
    : IQueryHandler<GetAccountsQuery, IReadOnlyList<AccountDto>>
{
    public async Task<Result<IReadOnlyList<AccountDto>>> HandleAsync(
        GetAccountsQuery query,
        CancellationToken ct = default)
    {
        var all = await accounts.GetAllAsync(query.ClientId, ct);

        return Result.Success(all);
    }
}

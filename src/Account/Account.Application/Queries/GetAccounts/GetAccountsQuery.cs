using Account.Application.Abstractions.Messaging;
using Account.Application.Dtos;

namespace Account.Application.Queries.GetAccounts;

// ClientId nulo trae todas las cuentas; con valor, solo las de ese cliente.
public sealed record GetAccountsQuery(Guid? ClientId) : IQuery<IReadOnlyList<AccountDto>>;

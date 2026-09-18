using Account.Application.Abstractions.Messaging;
using Account.Application.Dtos;

namespace Account.Application.Commands.CreateAccount;

public sealed record CreateAccountCommand(Guid ClientId, string Type, string Status) : ICommand<AccountDto>;

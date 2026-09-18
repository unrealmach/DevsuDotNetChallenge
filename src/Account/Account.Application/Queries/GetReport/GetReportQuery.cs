using Account.Application.Abstractions.Messaging;
using Account.Application.Dtos;

namespace Account.Application.Queries.GetReport;

public sealed record GetReportQuery(DateTime From, DateTime To, Guid? ClientId) : IQuery<IReadOnlyList<MovementDto>>;

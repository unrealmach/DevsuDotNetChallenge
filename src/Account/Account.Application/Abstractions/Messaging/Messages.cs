using Account.Application.Common;

namespace Account.Application.Abstractions.Messaging;

public interface IRequest<TResponse>;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;

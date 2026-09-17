namespace Identity.Application.Services;

internal interface IClientValidationService
{
    Task<bool> ClientIdExistsAsync(string clientId, CancellationToken ct = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
}

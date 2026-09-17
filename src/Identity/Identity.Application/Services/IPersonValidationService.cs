namespace Identity.Application.Services;

internal interface IPersonValidationService
{
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
}

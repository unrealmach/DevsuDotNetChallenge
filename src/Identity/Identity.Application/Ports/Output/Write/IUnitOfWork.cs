namespace Identity.Application.Ports.Output.Write;

internal interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

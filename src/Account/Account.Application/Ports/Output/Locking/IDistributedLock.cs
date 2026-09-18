namespace Account.Application.Ports.Output.Locking;

// Devuelve null si no se pudo adquirir el lock. Si devuelve algo, hay que
// mantenerlo en un "await using": al liberarlo (Dispose) se suelta el lock.
internal interface IDistributedLock
{
    Task<IAsyncDisposable?> AcquireAsync(string key, TimeSpan ttl, CancellationToken ct = default);
}

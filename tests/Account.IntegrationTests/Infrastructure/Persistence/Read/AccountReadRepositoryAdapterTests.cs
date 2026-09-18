using Account.Domain.Entities;
using Account.Infrastructure.Adapters.Persistence;
using Account.Infrastructure.Adapters.Persistence.Read;
using FluentAssertions;
using AccountEntity = Account.Domain.Entities.Account;

namespace Account.IntegrationTests.Infrastructure.Persistence.Read;

public class AccountReadRepositoryAdapterTests : IClassFixture<PostgresFixture>, IAsyncLifetime
{
    private readonly PostgresFixture _fixture;
    private AccountDbContext _context = null!;

    public AccountReadRepositoryAdapterTests(PostgresFixture fixture) => _fixture = fixture;

    public Task InitializeAsync()
    {
        _context = _fixture.CreateContext();
        return Task.CompletedTask;
    }

    public async Task DisposeAsync() => await _context.DisposeAsync();

    [Fact]
    public async Task GetAllAsync_ShouldIncludeTheClientName_NotJustTheAccountFields()
    {
        // Arrange — un cliente + cuenta + balance reales, insertados directo en la base.
        var client = Client.Create();
        client.WithName("Juan Perez");
        client.WithGender("M");
        client.WithAge("30");
        client.WithIdentification("0999999999");
        client.WithAddress("Calle 1");
        client.WithPhone("0987654321");
        client.WithClientId("juan.perez");
        client.WithStatus("ACTIVE");

        var account = AccountEntity.Create().WithClientId(client.Id).WithType("SAVINGS").WithStatus("ACTIVE");
        var balance = Balance.Create().ForAccount(account.Id);

        _context.Clients.Add(client);
        _context.Accounts.Add(account);
        _context.Balances.Add(balance);
        await _context.SaveChangesAsync();

        var repository = new AccountReadRepositoryAdapter(_context);

        // Act
        var accounts = await repository.GetAllAsync(client.Id, CancellationToken.None);

        // Assert
        // Este es el bug real que encontramos en vivo: sin el .Include(a => a.Client)
        // del adapter, esto tiraba NullReferenceException en vez de traer el nombre.
        accounts.Should().ContainSingle();
        accounts[0].ClientName.Should().Be("Juan Perez");
        accounts[0].Balance.Should().Be(0);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOnlyTheAccountsOfTheGivenClient_WhenFilteringByClientId()
    {
        // Arrange — dos clientes, cada uno con su propia cuenta.
        // Person.With... devuelve Person, no Client, asi que cada llamada va
        // por separado (no se puede encadenar cruzando esa frontera de tipos).
        var clientA = Client.Create();
        clientA.WithName("Cliente A");
        clientA.WithGender("F");
        clientA.WithAge("25");
        clientA.WithIdentification("111");
        clientA.WithAddress("Calle A");
        clientA.WithPhone("111");
        clientA.WithClientId("cliente.a");
        clientA.WithStatus("ACTIVE");

        var clientB = Client.Create();
        clientB.WithName("Cliente B");
        clientB.WithGender("M");
        clientB.WithAge("40");
        clientB.WithIdentification("222");
        clientB.WithAddress("Calle B");
        clientB.WithPhone("222");
        clientB.WithClientId("cliente.b");
        clientB.WithStatus("ACTIVE");

        var accountA = AccountEntity.Create().WithClientId(clientA.Id).WithType("CHECKING").WithStatus("ACTIVE");
        var accountB = AccountEntity.Create().WithClientId(clientB.Id).WithType("SAVINGS").WithStatus("ACTIVE");

        _context.Clients.AddRange(clientA, clientB);
        _context.Accounts.AddRange(accountA, accountB);
        _context.Balances.AddRange(Balance.Create().ForAccount(accountA.Id), Balance.Create().ForAccount(accountB.Id));
        await _context.SaveChangesAsync();

        var repository = new AccountReadRepositoryAdapter(_context);

        // Act
        var accounts = await repository.GetAllAsync(clientA.Id, CancellationToken.None);

        // Assert
        accounts.Should().ContainSingle();
        accounts[0].Id.Should().Be(accountA.Id);
    }
}

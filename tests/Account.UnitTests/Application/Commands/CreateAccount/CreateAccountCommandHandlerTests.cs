using Account.Application.Commands.CreateAccount;
using Account.Application.Dtos;
using Account.Application.Services;
using Account.Domain.Errors;
using FluentAssertions;
using Moq;
using AccountEntity = Account.Domain.Entities.Account;

namespace Account.UnitTests.Application.Commands.CreateAccount;

public class CreateAccountCommandHandlerTests
{
    private readonly Mock<IClientReadService> _clients = new();
    private readonly Mock<IAccountWriteService> _accounts = new();
    private readonly CreateAccountCommandHandler _handler;

    public CreateAccountCommandHandlerTests()
    {
        _handler = new CreateAccountCommandHandler(_clients.Object, _accounts.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnClientNotFound_WhenTheClientDoesNotExist()
    {
        // Arrange
        var command = new CreateAccountCommand(Guid.NewGuid(), "CHECKING", "ACTIVE");

        _clients.Setup(c => c.FindByIdAsync(command.ClientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ClientDto?)null);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(ErrorCatalog.ClientNotFound.Code);
    }

    [Fact]
    public async Task HandleAsync_ShouldNeverCreateAnAccount_WhenTheClientDoesNotExist()
    {
        // Arrange
        var command = new CreateAccountCommand(Guid.NewGuid(), "CHECKING", "ACTIVE");

        _clients.Setup(c => c.FindByIdAsync(command.ClientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ClientDto?)null);

        // Act
        await _handler.HandleAsync(command);

        // Assert
        // Sin cliente no hay a quien asociarle la cuenta: no debe llegar a escribirla.
        _accounts.Verify(
            a => a.CreateAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldCreateTheAccount_WithTheCommandsClientIdTypeAndStatus_WhenTheClientExists()
    {
        // Arrange
        var client = new ClientDto(
            Guid.NewGuid(), "Juan Perez", "M", "30", "0999999999",
            "Calle 1", "0987654321", "juan.perez", "ACTIVE", DateTime.UtcNow, null);
        var command = new CreateAccountCommand(client.Id, "SAVINGS", "ACTIVE");
        var account = AccountEntity.Create().WithClientId(client.Id).WithType(command.Type).WithStatus(command.Status);

        _clients.Setup(c => c.FindByIdAsync(client.Id, It.IsAny<CancellationToken>())).ReturnsAsync(client);
        _accounts.Setup(a => a.CreateAsync(client.Id, command.Type, command.Status, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        // Act
        await _handler.HandleAsync(command);

        // Assert
        _accounts.Verify(a => a.CreateAsync(client.Id, command.Type, command.Status, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnAnAccountDto_WithBalanceZero_WhenTheClientExists()
    {
        // Arrange
        var client = new ClientDto(
            Guid.NewGuid(), "Juan Perez", "M", "30", "0999999999",
            "Calle 1", "0987654321", "juan.perez", "ACTIVE", DateTime.UtcNow, null);
        var command = new CreateAccountCommand(client.Id, "SAVINGS", "ACTIVE");
        var account = AccountEntity.Create().WithClientId(client.Id).WithType(command.Type).WithStatus(command.Status);

        _clients.Setup(c => c.FindByIdAsync(client.Id, It.IsAny<CancellationToken>())).ReturnsAsync(client);
        _accounts.Setup(a => a.CreateAsync(client.Id, command.Type, command.Status, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new
        {
            Id = account.Id,
            ClientId = client.Id,
            ClientName = client.Name,
            Type = command.Type,
            Status = command.Status,
            Balance = 0
        });
    }
}

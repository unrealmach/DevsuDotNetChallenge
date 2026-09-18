using Account.Application.Commands.CreateAccount;
using FluentAssertions;

namespace Account.UnitTests.Application.Commands.CreateAccount;

public class CreateAccountCommandValidatorTests
{
    private readonly CreateAccountCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldReject_EmptyClientId()
    {
        // Arrange
        var command = new CreateAccountCommand(Guid.Empty, "CHECKING", "ACTIVE");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("CORRIENTE")]
    [InlineData("checking")]
    [InlineData("")]
    public void Validate_ShouldReject_TypeOutsideCheckingOrSavings(string type)
    {
        // Arrange
        var command = new CreateAccountCommand(Guid.NewGuid(), type, "ACTIVE");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("CHECKING")]
    [InlineData("SAVINGS")]
    public void Validate_ShouldAccept_TypeCheckingOrSavings(string type)
    {
        // Arrange
        var command = new CreateAccountCommand(Guid.NewGuid(), type, "ACTIVE");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("ACTIVA")]
    [InlineData("active")]
    [InlineData("")]
    public void Validate_ShouldReject_StatusOutsideActiveOrInactive(string status)
    {
        // Arrange
        var command = new CreateAccountCommand(Guid.NewGuid(), "CHECKING", status);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("ACTIVE")]
    [InlineData("INACTIVE")]
    public void Validate_ShouldAccept_StatusActiveOrInactive(string status)
    {
        // Arrange
        var command = new CreateAccountCommand(Guid.NewGuid(), "CHECKING", status);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}

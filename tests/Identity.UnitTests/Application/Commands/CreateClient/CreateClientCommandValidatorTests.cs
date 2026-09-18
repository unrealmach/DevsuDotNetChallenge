using FluentAssertions;
using Identity.Application.Commands.CreateClient;

namespace Identity.UnitTests.Application.Commands.CreateClient;

public class CreateClientCommandValidatorTests
{
    private readonly CreateClientCommandValidator _validator = new();

    [Theory]
    [InlineData("17")]
    [InlineData("18")]
    [InlineData("abc")]
    public void Validate_ShouldReject_InvalidAge(string age)
    {
        // Arrange
        var command = new CreateClientCommand(
            "Juan", "M", age, "12345", "Calle 1", "099", "juan.perez", "SuperSecreta1", "Activo");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_ShouldAccept_AgeGreaterThan18()
    {
        // Arrange
        var command = new CreateClientCommand(
            "Juan", "M", "19", "12345", "Calle 1", "099", "juan.perez", "SuperSecreta1", "ACTIVE");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("Activo")]
    [InlineData("active")]
    [InlineData("")]
    public void Validate_ShouldReject_StatusOutsideActiveOrInactive(string status)
    {
        // Arrange
        var command = new CreateClientCommand(
            "Juan", "M", "19", "12345", "Calle 1", "099", "juan.perez", "SuperSecreta1", status);

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
        var command = new CreateClientCommand(
            "Juan", "M", "19", "12345", "Calle 1", "099", "juan.perez", "SuperSecreta1", status);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}

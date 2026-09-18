using FluentAssertions;
using Identity.Application.Commands.CreateClient;

namespace Identity.UnitTests.Application.Commands.CreateClient;

public class CreateClientCommandValidatorTests
{
    private readonly CreateClientCommandValidator _validator = new();

    [Theory]
    [InlineData("16")]
    [InlineData("17")]
    [InlineData("abc")]
    public void Validate_ShouldReject_InvalidAge(string age)
    {
        // Arrange — Status valido a proposito: si fuera invalido, este test pasaria
        // igual aunque la regla de edad no rechazara nada (falso positivo real que
        // tuvimos: "18" quedo aceptado por la regla nueva y el test no lo noto
        // porque "Activo" tambien fallaba, por una razon que no era la que se probaba).
        var command = new CreateClientCommand(
            "Juan", "M", age, "12345", "Calle 1", "099", "juan.perez", "SuperSecreta1", "ACTIVE");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("18")]
    [InlineData("19")]
    public void Validate_ShouldAccept_AgeGreaterThan17(string age)
    {
        // Arrange
        var command = new CreateClientCommand(
            "Juan", "M", age, "12345", "Calle 1", "099", "juan.perez", "SuperSecreta1", "ACTIVE");

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

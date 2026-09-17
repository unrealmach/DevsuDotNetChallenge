using FluentAssertions;
using Identity.Application.Commands.CreatePerson;

namespace Identity.UnitTests.Application.Commands.CreatePerson;

public class CreatePersonCommandValidatorTests
{
    private readonly CreatePersonCommandValidator _validator = new();

    [Theory]
    [InlineData("17")]
    [InlineData("18")]
    [InlineData("abc")]
    public void Validate_ShouldReject_InvalidAge(string age)
    {
        // Arrange
        var command = new CreatePersonCommand("Juan", "M", age, "12345", "Calle 1", "099");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_ShouldAccept_AgeGreaterThan18()
    {
        // Arrange
        var command = new CreatePersonCommand("Juan", "M", "19", "12345", "Calle 1", "099");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}

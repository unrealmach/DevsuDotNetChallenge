using FluentAssertions;
using Identity.Domain.Entities;

namespace Identity.UnitTests.Domain.Entities;

public class ClientTests
{
    [Fact]
    public void Create_ShouldAssignANewNonEmptyId()
    {
        // Act
        var client = Client.Create();

        // Assert
        client.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Create_ShouldAssignADifferentId_ForEachInstance()
    {
        // Act
        var first = Client.Create();
        var second = Client.Create();

        // Assert
        first.Id.Should().NotBe(second.Id);
    }

    [Fact]
    public void Create_ShouldSetCreatedAtUtc_AndLeaveUpdatedAtUtcNull()
    {
        // Act
        var client = Client.Create();

        // Assert
        client.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        client.UpdatedAtUtc.Should().BeNull();
    }

    [Fact]
    public void WithClientId_ShouldSetTheValue_AndReturnTheSameInstance()
    {
        // Arrange
        var client = Client.Create();

        // Act
        var result = client.WithClientId("juan.perez");

        // Assert
        client.ClientId.Should().Be("juan.perez");
        result.Should().BeSameAs(client); // el patron fluido muta y devuelve this, no una copia
    }

    [Fact]
    public void Create_ShouldBuildAFullyPopulatedClient_WhenSettingAllTheFields()
    {
        // Act
        // Los With... de Person devuelven Person, no Client, asi que no se pueden
        // encadenar cruzando esa frontera en una sola expresion fluida (por eso
        // ClientWriteService tampoco encadena: llama cada With... por separado).
        var client = Client.Create();
        client.WithName("Juan Perez");
        client.WithGender("M");
        client.WithAge("30");
        client.WithIdentification("0999999999");
        client.WithAddress("Calle 1");
        client.WithPhone("0987654321");
        client.WithClientId("juan.perez");
        client.WithPassword("hash-no-importa-aca");
        client.WithStatus("Activo");

        // Assert
        client.Should().BeEquivalentTo(new
        {
            Name = "Juan Perez",
            Gender = "M",
            Age = "30",
            Identification = "0999999999",
            Address = "Calle 1",
            Phone = "0987654321",
            ClientId = "juan.perez",
            Password = "hash-no-importa-aca",
            Status = "Activo"
        });
    }

    [Fact]
    public void Touch_ShouldSetUpdatedAtUtc_ToNow()
    {
        // Arrange
        var client = Client.Create();

        // Act
        client.Touch();

        // Assert
        client.UpdatedAtUtc.Should().NotBeNull();
        client.UpdatedAtUtc!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}

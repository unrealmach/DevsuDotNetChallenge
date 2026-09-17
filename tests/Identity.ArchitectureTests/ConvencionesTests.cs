using NetArchTest.Rules;
using Xunit;

namespace Identity.ArchitectureTests;

public class ConvencionesTests
{
    [Fact]
    public void Handlers_ShouldBeInternalAndSealed()
    {
        // Arrange
        var handlers = Types.InAssembly(Layers.ApplicationAssembly)
            .That().HaveNameEndingWith("Handler");

        // Act
        var result = handlers.Should().NotBePublic().And().BeSealed().GetResult();

        // Assert
        result.Cumple("Un handler solo se alcanza por el mediator, asi que no se expone.");
    }

    [Fact]
    public void CommandsAndQueries_ShouldLiveNextToTheirHandler()
    {
        // Arrange
        var mensajes = Types.InAssembly(Layers.ApplicationAssembly)
            .That().AreClasses().And().HaveNameEndingWith("Command")
            .GetTypes()
            .Concat(Types.InAssembly(Layers.ApplicationAssembly)
                .That().AreClasses().And().HaveNameEndingWith("Query")
                .GetTypes());

        // Act
        var infractores = mensajes.Where(tipo =>
            tipo.Namespace?.StartsWith("Identity.Application.Commands") != true &&
            tipo.Namespace?.StartsWith("Identity.Application.Queries") != true);

        // Assert
        Layers.NoHayInfractores(infractores, "Cada mensaje vive en la carpeta de su caso de uso.");
    }

    [Fact]
    public void ConcreteServices_ShouldNotBeExposedOutsideTheCore()
    {
        // Arrange
        var servicios = Types.InAssembly(Layers.ApplicationAssembly)
            .That().ResideInNamespace(Layers.Services).And().AreClasses();

        // Act
        var result = servicios.Should().NotBePublic().GetResult();

        // Assert
        result.Cumple("Hacia afuera solo existen el mediator y los mensajes.");
    }

    [Fact]
    public void Adapters_ShouldBeInternalAndSealed()
    {
        // Arrange
        var adaptadores = Types.InAssembly(Layers.InfrastructureAssembly)
            .That().HaveNameEndingWith("Adapter");

        // Act
        var result = adaptadores.Should().NotBePublic().And().BeSealed().GetResult();

        // Assert
        result.Cumple("Un adaptador se enchufa por DI; nadie lo referencia por su nombre.");
    }
}

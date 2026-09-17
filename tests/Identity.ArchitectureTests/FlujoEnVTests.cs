using NetArchTest.Rules;
using Xunit;

namespace Identity.ArchitectureTests;

public class FlujoEnVTests
{
    [Fact]
    public void Domain_ShouldNotDependOnAnyOtherLayerOrTechnology()
    {
        // Arrange
        var dominio = Types.InAssembly(Layers.DomainAssembly);

        // Act
        var result = dominio.Should()
            .NotHaveDependencyOnAny(
                Layers.Application,
                Layers.Infrastructure,
                Layers.Api,
                "Microsoft.EntityFrameworkCore",
                "Microsoft.AspNetCore",
                "Npgsql")
            .GetResult();

        // Assert
        result.Cumple("El centro del hexagono no conoce a nadie: ni capas externas ni frameworks.");
    }

    [Fact]
    public void Application_ShouldNotKnowInfrastructureOrApi()
    {
        // Arrange
        var aplicacion = Types.InAssembly(Layers.ApplicationAssembly);

        // Act
        var result = aplicacion.Should()
            .NotHaveDependencyOnAny(Layers.Infrastructure, Layers.Api)
            .GetResult();

        // Assert
        result.Cumple("El nucleo define puertos; jamas conoce a quien los implementa.");
    }

    [Fact]
    public void Application_ShouldNotKnowEfCoreNpgsqlOrAspNetCore()
    {
        // Arrange
        var aplicacion = Types.InAssembly(Layers.ApplicationAssembly);

        // Act
        var result = aplicacion.Should()
            .NotHaveDependencyOnAny("Microsoft.EntityFrameworkCore", "Npgsql", "Microsoft.AspNetCore")
            .GetResult();

        // Assert
        result.Cumple("El nucleo no puede atarse a una tecnologia de persistencia ni de transporte.");
    }

    [Fact]
    public void Infrastructure_ShouldNotKnowApi()
    {
        // Arrange
        var infraestructura = Types.InAssembly(Layers.InfrastructureAssembly);

        // Act
        var result = infraestructura.Should().NotHaveDependencyOn(Layers.Api).GetResult();

        // Assert
        result.Cumple("Los adaptadores de salida no saben que existe HTTP.");
    }

    [Fact]
    public void Api_ShouldNotReachPortsOrServices()
    {
        // Arrange
        var api = Types.InAssembly(Layers.ApiAssembly);

        // Act
        var result = api.Should().NotHaveDependencyOnAny(Layers.Ports, Layers.Services).GetResult();

        // Assert
        result.Cumple("El adaptador de entrada entra por el mediator; no baja dos escalones de golpe.");
    }

    [Fact]
    public void Controllers_ShouldOnlyTalkToTheMediator()
    {
        // Arrange
        var controllers = Types.InAssembly(Layers.ApiAssembly)
            .That().ResideInNamespace(Layers.Controllers);

        // Act
        var result = controllers.Should()
            .NotHaveDependencyOnAny(
                Layers.Ports,
                Layers.Services,
                Layers.Infrastructure,
                "Identity.Application.Commands.CreateClient.CreateClientCommandHandler",
                "Microsoft.EntityFrameworkCore")
            .GetResult();

        // Assert
        result.Cumple("Un controller arma el mensaje y lo entrega; no orquesta ni persiste.");
    }

    [Fact]
    public void Handlers_ShouldOrchestrateServicesAndNeverTouchAPort()
    {
        // Arrange
        var handlers = Types.InAssembly(Layers.ApplicationAssembly)
            .That().HaveNameEndingWith("Handler")
            .And().HaveDependencyOn(Layers.Ports);

        // Act
        var infractores = handlers.GetTypes();

        // Assert
        Layers.NoHayInfractores(
            infractores,
            "El handler baja por los servicios; el puerto lo toca el servicio, no el.");
    }

    [Fact]
    public void OnlyServicesAndThePipeline_ShouldTouchPorts()
    {
        // Arrange
        var consumidoresDePuertos = Types.InAssembly(Layers.ApplicationAssembly)
            .That().HaveDependencyOn(Layers.Ports)
            .And().DoNotResideInNamespace(Layers.Services)
            .And().DoNotResideInNamespace(Layers.Behaviors)
            .And().DoNotResideInNamespace(Layers.Ports)
            .And().DoNotHaveName("DependencyInjection");

        // Act
        var infractores = consumidoresDePuertos.GetTypes();

        // Assert
        Layers.NoHayInfractores(
            infractores,
            "Los puertos tienen un unico consumidor: la capa de servicios (y el behavior que cierra la transaccion).");
    }

    [Fact]
    public void OnlyTheCompositionRoot_ShouldKnowInfrastructure()
    {
        // Arrange
        var dependenDeInfraestructura = Types.InAssembly(Layers.ApiAssembly)
            .That().HaveDependencyOn(Layers.Infrastructure)
            .And().DoNotHaveName("Program");

        // Act
        var infractores = dependenDeInfraestructura.GetTypes();

        // Assert
        Layers.NoHayInfractores(
            infractores,
            "Solo el arranque enchufa adaptadores; el resto del Api ignora que existe Postgres.");
    }
}

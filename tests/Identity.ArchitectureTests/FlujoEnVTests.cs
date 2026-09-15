using NetArchTest.Rules;
using Xunit;

namespace Identity.ArchitectureTests;

public class FlujoEnVTests
{
    [Fact]
    public void El_dominio_no_depende_de_ninguna_otra_capa_ni_de_tecnologia()
    {
        Types.InAssembly(Layers.DomainAssembly)
            .Should()
            .NotHaveDependencyOnAny(
                Layers.Application,
                Layers.Infrastructure,
                Layers.Api,
                "Microsoft.EntityFrameworkCore",
                "Microsoft.AspNetCore",
                "Npgsql")
            .GetResult()
            .Cumple("El centro del hexagono no conoce a nadie: ni capas externas ni frameworks.");
    }

    [Fact]
    public void La_aplicacion_no_conoce_la_infraestructura_ni_el_api()
    {
        Types.InAssembly(Layers.ApplicationAssembly)
            .Should()
            .NotHaveDependencyOnAny(Layers.Infrastructure, Layers.Api)
            .GetResult()
            .Cumple("El nucleo define puertos; jamas conoce a quien los implementa.");
    }

    [Fact]
    public void La_aplicacion_no_conoce_EFCore_Npgsql_ni_AspNetCore()
    {
        Types.InAssembly(Layers.ApplicationAssembly)
            .Should()
            .NotHaveDependencyOnAny("Microsoft.EntityFrameworkCore", "Npgsql", "Microsoft.AspNetCore")
            .GetResult()
            .Cumple("El nucleo no puede atarse a una tecnologia de persistencia ni de transporte.");
    }

    [Fact]
    public void La_infraestructura_no_conoce_el_api()
    {
        Types.InAssembly(Layers.InfrastructureAssembly)
            .Should()
            .NotHaveDependencyOn(Layers.Api)
            .GetResult()
            .Cumple("Los adaptadores de salida no saben que existe HTTP.");
    }

    [Fact]
    public void El_api_no_alcanza_los_puertos_ni_los_servicios()
    {
        Types.InAssembly(Layers.ApiAssembly)
            .Should()
            .NotHaveDependencyOnAny(Layers.Ports, Layers.Services)
            .GetResult()
            .Cumple("El adaptador de entrada entra por el mediator; no baja dos escalones de golpe.");
    }

    [Fact]
    public void Los_controllers_solo_hablan_con_el_mediator()
    {
        Types.InAssembly(Layers.ApiAssembly)
            .That().ResideInNamespace(Layers.Controllers)
            .Should()
            .NotHaveDependencyOnAny(
                Layers.Ports,
                Layers.Services,
                Layers.Infrastructure,
                "Identity.Application.Commands.RegisterUser.RegisterUserCommandHandler",
                "Microsoft.EntityFrameworkCore")
            .GetResult()
            .Cumple("Un controller arma el mensaje y lo entrega; no orquesta ni persiste.");
    }

    [Fact]
    public void Los_handlers_orquestan_servicios_y_nunca_tocan_un_puerto()
    {
        var infractores = Types.InAssembly(Layers.ApplicationAssembly)
            .That().HaveNameEndingWith("Handler")
            .And().HaveDependencyOn(Layers.Ports)
            .GetTypes();

        Layers.NoHayInfractores(
            infractores,
            "El handler baja por los servicios; el puerto lo toca el servicio, no el.");
    }

    [Fact]
    public void Solo_los_servicios_y_el_pipeline_tocan_los_puertos()
    {
        var infractores = Types.InAssembly(Layers.ApplicationAssembly)
            .That().HaveDependencyOn(Layers.Ports)
            .And().DoNotResideInNamespace(Layers.Services)
            .And().DoNotResideInNamespace(Layers.Behaviors)
            .And().DoNotResideInNamespace(Layers.Ports)
            .And().DoNotHaveName("DependencyInjection")
            .GetTypes();

        Layers.NoHayInfractores(
            infractores,
            "Los puertos tienen un unico consumidor: la capa de servicios (y el behavior que cierra la transaccion).");
    }

    [Fact]
    public void Solo_el_composition_root_conoce_la_infraestructura()
    {
        var infractores = Types.InAssembly(Layers.ApiAssembly)
            .That().HaveDependencyOn(Layers.Infrastructure)
            .And().DoNotHaveName("Program")
            .And().DoNotHaveName("JwtAuthenticationExtensions")
            .GetTypes();

        Layers.NoHayInfractores(
            infractores,
            "Solo el arranque enchufa adaptadores; el resto del Api ignora que existe Postgres.");
    }
}

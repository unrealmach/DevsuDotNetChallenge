using NetArchTest.Rules;
using Xunit;

namespace Identity.ArchitectureTests;

public class ConvencionesTests
{
    [Fact]
    public void Los_handlers_son_internal_y_sellados()
    {
        Types.InAssembly(Layers.ApplicationAssembly)
            .That().HaveNameEndingWith("Handler")
            .Should().NotBePublic().And().BeSealed()
            .GetResult()
            .Cumple("Un handler solo se alcanza por el mediator, asi que no se expone.");
    }

    [Fact]
    public void Los_comandos_y_consultas_viven_junto_a_su_handler()
    {
        var mensajes = Types.InAssembly(Layers.ApplicationAssembly)
            .That().AreClasses().And().HaveNameEndingWith("Command")
            .GetTypes()
            .Concat(Types.InAssembly(Layers.ApplicationAssembly)
                .That().AreClasses().And().HaveNameEndingWith("Query")
                .GetTypes());

        var infractores = mensajes.Where(tipo =>
            tipo.Namespace?.StartsWith("Identity.Application.Commands") != true &&
            tipo.Namespace?.StartsWith("Identity.Application.Queries") != true);

        Layers.NoHayInfractores(infractores, "Cada mensaje vive en la carpeta de su caso de uso.");
    }

    [Fact]
    public void Los_servicios_concretos_no_se_exponen_fuera_del_nucleo()
    {
        Types.InAssembly(Layers.ApplicationAssembly)
            .That().ResideInNamespace(Layers.Services).And().AreClasses()
            .Should().NotBePublic()
            .GetResult()
            .Cumple("Hacia afuera solo existen el mediator y los mensajes.");
    }

    [Fact]
    public void Los_adaptadores_son_internal_y_sellados()
    {
        Types.InAssembly(Layers.InfrastructureAssembly)
            .That().HaveNameEndingWith("Adapter")
            .Should().NotBePublic().And().BeSealed()
            .GetResult()
            .Cumple("Un adaptador se enchufa por DI; nadie lo referencia por su nombre.");
    }
}

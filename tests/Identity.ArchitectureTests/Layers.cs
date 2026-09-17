using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace Identity.ArchitectureTests;

public static class Layers
{
    public const string Domain = "Identity.Domain";
    public const string Application = "Identity.Application";
    public const string Infrastructure = "Identity.Infrastructure";
    public const string Api = "Identity.Api";

    public const string Ports = "Identity.Application.Ports.Output";
    public const string Services = "Identity.Application.Services";
    public const string Behaviors = "Identity.Application.Mediator.Behaviors";
    public const string Controllers = "Identity.Api.Controllers";

    public static readonly Assembly DomainAssembly = typeof(Identity.Domain.Entities.Person).Assembly;
    public static readonly Assembly ApplicationAssembly = typeof(Identity.Application.DependencyInjection).Assembly;
    public static readonly Assembly InfrastructureAssembly = typeof(Identity.Infrastructure.DependencyInjection).Assembly;
    public static readonly Assembly ApiAssembly = typeof(Identity.Api.Controllers.ClientsController).Assembly;

    public static void Cumple(this TestResult result, string regla)
    {
        var infractores = result.FailingTypeNames is null ? string.Empty : string.Join("\n  - ", result.FailingTypeNames);

        Assert.True(result.IsSuccessful, $"{regla}\nTipos que la incumplen:\n  - {infractores}");
    }

    public static void NoHayInfractores(IEnumerable<Type> tipos, string regla)
    {
        var infractores = tipos.Select(t => t.FullName).ToList();

        Assert.True(infractores.Count == 0, $"{regla}\nTipos que la incumplen:\n  - {string.Join("\n  - ", infractores)}");
    }
}

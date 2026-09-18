using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace Account.ArchitectureTests;

public static class Layers
{
    public const string Domain = "Account.Domain";
    public const string Application = "Account.Application";
    public const string Infrastructure = "Account.Infrastructure";
    public const string Api = "Account.Api";

    public const string Ports = "Account.Application.Ports.Output";
    public const string Services = "Account.Application.Services";
    public const string Behaviors = "Account.Application.Mediator.Behaviors";
    public const string Controllers = "Account.Api.Controllers";

    public static readonly Assembly DomainAssembly = typeof(Account.Domain.Errors.ErrorCatalog).Assembly;
    public static readonly Assembly ApplicationAssembly = typeof(Account.Application.DependencyInjection).Assembly;
    public static readonly Assembly InfrastructureAssembly = typeof(Account.Infrastructure.DependencyInjection).Assembly;
    public static readonly Assembly ApiAssembly = typeof(Program).Assembly;

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

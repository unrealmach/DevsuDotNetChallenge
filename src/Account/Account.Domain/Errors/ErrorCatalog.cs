using System.Reflection;

namespace Account.Domain.Errors;

public static class ErrorCatalog
{
    public const string GenericResponseMessage = "Ocurrio un error procesando la solicitud. Contacta a soporte con el traceId.";

    public static readonly ErrorDefinition UseCaseFailure = new(
        "ACC-APP-999",
        ErrorLayer.Application,
        ErrorSeverity.Error,
        ErrorExposure.LogOnly,
        500,
        "Se leyo el valor de un resultado fallido: error de programacion en un caso de uso");

    public static readonly ErrorDefinition HandlerNotRegistered = new(
        "ACC-APP-998",
        ErrorLayer.Application,
        ErrorSeverity.Critical,
        ErrorExposure.LogOnly,
        500,
        "No hay handler registrado para el comando o la consulta despachada");

    public static readonly ErrorDefinition DatabaseUnavailable = new(
        "ACC-INF-001",
        ErrorLayer.Infrastructure,
        ErrorSeverity.Critical,
        ErrorExposure.LogOnly,
        503,
        "La base de datos no responde");

    public static readonly ErrorDefinition DatabaseConcurrencyConflict = new(
        "ACC-INF-002",
        ErrorLayer.Infrastructure,
        ErrorSeverity.Error,
        ErrorExposure.LogOnly,
        409,
        "Conflicto de concurrencia al guardar cambios");

    public static readonly ErrorDefinition DatabaseConstraintViolation = new(
        "ACC-INF-003",
        ErrorLayer.Infrastructure,
        ErrorSeverity.Error,
        ErrorExposure.LogOnly,
        409,
        "Violacion de una restriccion de la base de datos");

    public static readonly ErrorDefinition DatabaseMigrationFailed = new(
        "ACC-INF-004",
        ErrorLayer.Infrastructure,
        ErrorSeverity.Critical,
        ErrorExposure.LogOnly,
        500,
        "Fallo la aplicacion de migraciones en el arranque");

    public static readonly ErrorDefinition RequestValidationFailed = new(
        "ACC-API-001",
        ErrorLayer.Api,
        ErrorSeverity.Information,
        ErrorExposure.Response,
        400,
        "La peticion no supero la validacion de contratos",
        "La peticion contiene campos invalidos.");

    public static readonly ErrorDefinition MalformedRequestBody = new(
        "ACC-API-002",
        ErrorLayer.Api,
        ErrorSeverity.Information,
        ErrorExposure.Response,
        400,
        "El cuerpo de la peticion no se pudo deserializar",
        "El cuerpo de la peticion esta mal formado.");

    public static readonly ErrorDefinition Unhandled = new(
        "ACC-API-999",
        ErrorLayer.Api,
        ErrorSeverity.Critical,
        ErrorExposure.LogOnly,
        500,
        "Excepcion no controlada que llego al borde de la aplicacion");

    public static IReadOnlyList<ErrorDefinition> All { get; } = typeof(ErrorCatalog)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(field => field.FieldType == typeof(ErrorDefinition))
        .Select(field => (ErrorDefinition)field.GetValue(null)!)
        .OrderBy(definition => definition.Code, StringComparer.Ordinal)
        .ToList();

    public static ErrorDefinition? Find(string code) =>
        All.FirstOrDefault(definition => definition.Code == code);
}
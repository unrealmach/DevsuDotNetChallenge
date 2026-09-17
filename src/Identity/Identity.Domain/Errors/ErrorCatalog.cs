using System.Reflection;

namespace Identity.Domain.Errors;

public static class ErrorCatalog
{
    public const string GenericResponseMessage = "Ocurrio un error procesando la solicitud. Contacta a soporte con el traceId.";

    public static readonly ErrorDefinition PersonNotFound = new(
        "IDN-APP-006",
        ErrorLayer.Application,
        ErrorSeverity.Warning,
        ErrorExposure.Response,
        404,
        "La persona solicitada no existe",
        "Persona no encontrada.");

    public static readonly ErrorDefinition UseCaseFailure = new(
        "IDN-APP-999",
        ErrorLayer.Application,
        ErrorSeverity.Error,
        ErrorExposure.LogOnly,
        500,
        "Se leyo el valor de un resultado fallido: error de programacion en un caso de uso");

    public static readonly ErrorDefinition HandlerNotRegistered = new(
        "IDN-APP-998",
        ErrorLayer.Application,
        ErrorSeverity.Critical,
        ErrorExposure.LogOnly,
        500,
        "No hay handler registrado para el comando o la consulta despachada");

    public static readonly ErrorDefinition DatabaseUnavailable = new(
        "IDN-INF-001",
        ErrorLayer.Infrastructure,
        ErrorSeverity.Critical,
        ErrorExposure.LogOnly,
        503,
        "La base de datos no responde");

    public static readonly ErrorDefinition DatabaseConcurrencyConflict = new(
        "IDN-INF-002",
        ErrorLayer.Infrastructure,
        ErrorSeverity.Error,
        ErrorExposure.LogOnly,
        409,
        "Conflicto de concurrencia al guardar cambios");

    public static readonly ErrorDefinition DatabaseConstraintViolation = new(
        "IDN-INF-003",
        ErrorLayer.Infrastructure,
        ErrorSeverity.Error,
        ErrorExposure.LogOnly,
        409,
        "Violacion de una restriccion de la base de datos");

    public static readonly ErrorDefinition DatabaseMigrationFailed = new(
        "IDN-INF-004",
        ErrorLayer.Infrastructure,
        ErrorSeverity.Critical,
        ErrorExposure.LogOnly,
        500,
        "Fallo la aplicacion de migraciones en el arranque");

    public static readonly ErrorDefinition RequestValidationFailed = new(
        "IDN-API-001",
        ErrorLayer.Api,
        ErrorSeverity.Information,
        ErrorExposure.Response,
        400,
        "La peticion no supero la validacion de contratos",
        "La peticion contiene campos invalidos.");

    public static readonly ErrorDefinition MissingOrInvalidToken = new(
        "IDN-API-002",
        ErrorLayer.Api,
        ErrorSeverity.Information,
        ErrorExposure.Response,
        401,
        "Peticion sin access token o con un token invalido",
        "Se requiere un access token valido.");

    public static readonly ErrorDefinition Forbidden = new(
        "IDN-API-003",
        ErrorLayer.Api,
        ErrorSeverity.Warning,
        ErrorExposure.Response,
        403,
        "El usuario autenticado no tiene permiso sobre el recurso",
        "No tienes permiso para realizar esta accion.");

    public static readonly ErrorDefinition MalformedRequestBody = new(
        "IDN-API-004",
        ErrorLayer.Api,
        ErrorSeverity.Information,
        ErrorExposure.Response,
        400,
        "El cuerpo de la peticion no se pudo deserializar",
        "El cuerpo de la peticion esta mal formado.");

    public static readonly ErrorDefinition Unhandled = new(
        "IDN-API-999",
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

using System.Reflection;

namespace Identity.Domain.Errors;

public static class ErrorCatalog
{
    public const string GenericResponseMessage = "Ocurrio un error procesando la solicitud. Contacta a soporte con el traceId.";

    public static readonly ErrorDefinition UserInvalidEmail = new(
        "IDN-DOM-001",
        ErrorLayer.Domain,
        ErrorSeverity.Warning,
        ErrorExposure.Response,
        400,
        "El correo no cumple el formato exigido por el dominio",
        "El correo electronico no tiene un formato valido.");

    public static readonly ErrorDefinition UserInvalidName = new(
        "IDN-DOM-002",
        ErrorLayer.Domain,
        ErrorSeverity.Warning,
        ErrorExposure.Response,
        400,
        "El nombre del usuario esta vacio",
        "El nombre del usuario es obligatorio.");

    public static readonly ErrorDefinition UserInvalidRole = new(
        "IDN-DOM-003",
        ErrorLayer.Domain,
        ErrorSeverity.Error,
        ErrorExposure.LogOnly,
        500,
        "Se intento crear un usuario con un rol no soportado");

    public static readonly ErrorDefinition UserEmptyPasswordHash = new(
        "IDN-DOM-004",
        ErrorLayer.Domain,
        ErrorSeverity.Error,
        ErrorExposure.LogOnly,
        500,
        "Se intento crear un usuario sin hash de password");

    public static readonly ErrorDefinition RefreshTokenInvalidExpiration = new(
        "IDN-DOM-005",
        ErrorLayer.Domain,
        ErrorSeverity.Error,
        ErrorExposure.LogOnly,
        500,
        "Se intento emitir un refresh token con expiracion en el pasado");

    public static readonly ErrorDefinition RefreshTokenAlreadyRevoked = new(
        "IDN-DOM-006",
        ErrorLayer.Domain,
        ErrorSeverity.Warning,
        ErrorExposure.Response,
        409,
        "Se intento revocar un refresh token ya revocado",
        "La sesion ya habia sido revocada.");

    public static readonly ErrorDefinition UserEmailAlreadyRegistered = new(
        "IDN-APP-001",
        ErrorLayer.Application,
        ErrorSeverity.Warning,
        ErrorExposure.Response,
        409,
        "Intento de registro con un correo ya existente",
        "Ya existe un usuario registrado con ese correo.");

    public static readonly ErrorDefinition InvalidCredentials = new(
        "IDN-APP-002",
        ErrorLayer.Application,
        ErrorSeverity.Warning,
        ErrorExposure.Response,
        401,
        "Intento de login con credenciales invalidas",
        "Credenciales invalidas.");

    public static readonly ErrorDefinition UserInactive = new(
        "IDN-APP-003",
        ErrorLayer.Application,
        ErrorSeverity.Warning,
        ErrorExposure.Response,
        401,
        "Intento de acceso de un usuario inactivo",
        "El usuario esta inactivo.");

    public static readonly ErrorDefinition InvalidRefreshToken = new(
        "IDN-APP-004",
        ErrorLayer.Application,
        ErrorSeverity.Warning,
        ErrorExposure.Response,
        401,
        "Refresh token inexistente, expirado o revocado",
        "El refresh token es invalido o expiro.");

    public static readonly ErrorDefinition UserNotFound = new(
        "IDN-APP-005",
        ErrorLayer.Application,
        ErrorSeverity.Warning,
        ErrorExposure.Response,
        404,
        "El usuario solicitado no existe",
        "Usuario no encontrado.");

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

    public static readonly ErrorDefinition TokenSigningFailed = new(
        "IDN-INF-005",
        ErrorLayer.Infrastructure,
        ErrorSeverity.Critical,
        ErrorExposure.LogOnly,
        500,
        "No se pudo firmar el access token: revisa la configuracion Jwt");

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

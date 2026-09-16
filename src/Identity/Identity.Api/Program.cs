using Identity.Api.Errors;
using Identity.Api.Extensions;
using Identity.Api.Middleware;
using Identity.Application;
using Identity.Infrastructure;
using Identity.Domain.Errors;
using Identity.Infrastructure.Adapters.Persistence;
using Identity.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(entry => entry.Value is { Errors.Count: > 0 })
            .ToDictionary(
                entry => entry.Key,
                entry => entry.Value!.Errors
                    .Select(error => error.ErrorMessage.Length > 0
                        ? error.ErrorMessage
                        : error.Exception?.Message ?? "Valor invalido.")
                    .ToArray());

        var response = ErrorResponse.From(ErrorCatalog.RequestValidationFailed, context.HttpContext) with
        {
            Errors = errors
        };

        return new BadRequestObjectResult(response);
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Devsu Identity API",
        Version = "v1",
        Description = "Microservicio de autenticacion y gestion de usuarios (arquitectura hexagonal)."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Pega aqui el access token (sin el prefijo 'Bearer')."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHealthChecks();

builder.Services.AddIdentityObservability(builder.Configuration);
builder.Logging.AddIdentityObservability(builder.Configuration);

builder.Services.AddIdentityApplication();
builder.Services.AddIdentityInfrastructure(
    builder.Configuration.GetConnectionString("Identity")
    ?? throw new InvalidOperationException("Falta la cadena de conexion 'Identity'."),
    builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

if (app.Configuration.GetValue<bool>("RunMigrationsOnStartup"))
{
    using var scope = app.Services.CreateScope();

    try
    {
        await scope.ServiceProvider.GetRequiredService<IdentityDbContext>().Database.MigrateAsync();
    }
    catch (Exception exception)
    {
        var failure = new InfrastructureException(ErrorCatalog.DatabaseMigrationFailed, exception.Message, exception);
        app.Services.GetRequiredService<ILoggerFactory>()
            .CreateLogger("Identity.Api.Startup")
            .Log(failure.Error.Severity.ToLogLevel(), failure, "{ErrorCode} {ErrorLayer} {ErrorMessage}",
                failure.Error.Code, failure.Error.Layer, failure.Error.LogMessage);

        throw failure;
    }
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity API v1"));
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health").AllowAnonymous();

app.Run();

public partial class Program;

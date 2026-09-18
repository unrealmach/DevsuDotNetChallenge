using Account.Api.Errors;
using Account.Api.Extensions;
using Account.Api.Middleware;
using Account.Application;
using Account.Infrastructure;
using Account.Domain.Errors;
using Account.Infrastructure.Adapters.Persistence;
using Account.Infrastructure.Exceptions;
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
        Title = "Account API",
        Version = "v1",
        Description = "Microservicio Account."
    });
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHealthChecks();

builder.Services.AddAccountObservability(builder.Configuration);
builder.Logging.AddAccountObservability(builder.Configuration);

builder.Services.AddAccountApplication();
builder.Services.AddAccountInfrastructure(
    builder.Configuration.GetConnectionString("Account")
    ?? throw new InvalidOperationException("Falta la cadena de conexion 'Account'."),
    builder.Configuration);

var app = builder.Build();

if (app.Configuration.GetValue<bool>("RunMigrationsOnStartup"))
{
    using var scope = app.Services.CreateScope();

    try
    {
        await scope.ServiceProvider.GetRequiredService<AccountDbContext>().Database.MigrateAsync();
    }
    catch (Exception exception)
    {
        var failure = new InfrastructureException(ErrorCatalog.DatabaseMigrationFailed, exception.Message, exception);
        app.Services.GetRequiredService<ILoggerFactory>()
            .CreateLogger("Account.Api.Startup")
            .Log(failure.Error.Severity.ToLogLevel(), failure, "{ErrorCode} {ErrorLayer} {ErrorMessage}",
                failure.Error.Code, failure.Error.Layer, failure.Error.LogMessage);

        throw failure;
    }
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Account API v1"));
}

app.MapControllers();
app.MapHealthChecks("/health").AllowAnonymous();

app.Run();

public partial class Program;

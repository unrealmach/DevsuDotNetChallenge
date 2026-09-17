using System.ComponentModel.DataAnnotations;
using Identity.Application.Commands.CreateClient;
using Identity.Application.Commands.PatchClient;
using Identity.Application.Commands.UpdateClient;

namespace Identity.Api.Contracts;

public sealed record CreateClientRequest
{
    //data anotations
    [Required]
    [MaxLength(160)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string Gender { get; init; } = string.Empty;

    [Required]
    [MaxLength(5)]
    public string Age { get; init; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Identification { get; init; } = string.Empty;

    [Required]
    public string Address { get; init; } = string.Empty;

    [Required]
    public string Phone { get; init; } = string.Empty;

    [Required]
    [MaxLength(160)]
    public string ClientId { get; init; } = string.Empty;

    [Required]
    [MinLength(8)]
    [MaxLength(160)]
    public string Password { get; init; } = string.Empty;

    [Required]
    [MaxLength(160)]
    public string Status { get; init; } = string.Empty;

    public CreateClientCommand ToCommand() =>
        new(Name, Gender, Age, Identification, Address, Phone, ClientId, Password, Status);
}

public sealed record UpdateClientRequest
{
    [Required]
    [MaxLength(160)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string Gender { get; init; } = string.Empty;

    [Required]
    [MaxLength(5)]
    public string Age { get; init; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Identification { get; init; } = string.Empty;

    [Required]
    public string Address { get; init; } = string.Empty;

    [Required]
    public string Phone { get; init; } = string.Empty;

    [Required]
    [MaxLength(160)]
    public string Status { get; init; } = string.Empty;

    public UpdateClientCommand ToCommand(Guid id) =>
        new(id, Name, Gender, Age, Identification, Address, Phone, Status);
}

public sealed record PatchClientRequest
{
    [MaxLength(160)]
    public string? Name { get; init; }

    [MaxLength(10)]
    public string? Gender { get; init; }

    [MaxLength(5)]
    public string? Age { get; init; }

    [MaxLength(20)]
    public string? Identification { get; init; }

    public string? Address { get; init; }

    public string? Phone { get; init; }

    [MinLength(8)]
    [MaxLength(160)]
    public string? Password { get; init; }

    [MaxLength(160)]
    public string? Status { get; init; }

    public PatchClientCommand ToCommand(Guid id) =>
        new(id, Name, Gender, Age, Identification, Address, Phone, Password, Status);
}

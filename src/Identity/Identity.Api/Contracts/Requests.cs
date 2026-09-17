using System.ComponentModel.DataAnnotations;
using Identity.Application.Commands.CreatePerson;
using Identity.Application.Commands.UpdatePerson;

namespace Identity.Api.Contracts;

public sealed record CreatePersonRequest
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

    public CreatePersonCommand ToCommand() => new(Name, Gender, Age, Identification, Address, Phone);
}

public sealed record UpdatePersonRequest
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

    public UpdatePersonCommand ToCommand(Guid id) => new(id, Name, Gender, Age, Identification, Address, Phone);
}

using Account.Domain.Common;

namespace Account.Domain.Entities;

public class Person : AuditableEntity
{
    protected Person()
    {
        Id = Guid.NewGuid();
    }

    protected Person(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Gender { get; private set; } = null!;
    public string Age { get; private set; } = null!;
    public string Identification { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string Phone { get; private set; } = null!;

    public static Person Create() => new();

    // Para replicar una entidad que ya existe en otro microservicio (via evento de
    // integracion) y necesita conservar el mismo Id, no uno nuevo.
    public static Person CreateWithId(Guid id) => new(id);

    public Person WithName(string name)
    {
        Name = name;
        return this;
    }

    public Person WithGender(string gender)
    {
        Gender = gender;
        return this;
    }

    public Person WithAge(string age)
    {
        Age = age;
        return this;
    }

    public Person WithIdentification(string identification)
    {
        Identification = identification;
        return this;
    }

    public Person WithAddress(string address)
    {
        Address = address;
        return this;
    }

    public Person WithPhone(string phone)
    {
        Phone = phone;
        return this;
    }

    public Person Touch()
    {
        MarkAsUpdated();
        return this;
    }
}

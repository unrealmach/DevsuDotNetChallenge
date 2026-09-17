using Identity.Domain.Common;

namespace Identity.Domain.Entities;

public class Person : AuditableEntity
{
    protected Person()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Gender { get; private set; } = null!;
    public string Age { get; private set; } = null!;
    public string Identification { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string Phone { get; private set; } = null!;

    public static Person Create() => new();

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

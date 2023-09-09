namespace GrpcService.Models;

public class Person
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public IReadOnlyList<Address> Addresses { get; init; }

    public Address MainAddress { get; init; }
}
namespace GraphQLApplication.Models;

[Node(
    IdField = nameof(Id),
    NodeResolverType = typeof(Person),
    NodeResolver = nameof(PersonResolver.ResolveAsync))]
public class Person
{
    [ID] public Guid Id { get; init; }

    public string Name { get; init; }

    public IReadOnlyList<Address> Addresses { get; init; }

    public Address MainAddress { get; init; }
}
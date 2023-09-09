using MongoDB.Bson.Serialization.Attributes;

namespace GrpcService.Models;

public class Address
{
    [BsonIgnore] public Guid Id { get; init; }

    public required string Street { get; init; }
    public required string City { get; init; }
}
using GraphQLApplication.Models;
using HotChocolate.Data;
using MongoDB.Driver;

namespace GraphQLApplication.Types;

public class Query
{
    [UsePaging]
    [UseProjection]
    [UseSorting]
    [UseFiltering]
    public IExecutable<Person> GetPersons(
        [Service] IMongoCollection<Person> collection)
    {
        return collection.AsExecutable();
    }

    [UseFirstOrDefault]
    public IExecutable<Person> GetPersonById(
        [Service] IMongoCollection<Person> collection,
        [ID] Guid id)
    {
        return collection.Find(x => x.Id == id).AsExecutable();
    }
}
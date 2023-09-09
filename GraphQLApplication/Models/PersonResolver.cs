using MongoDB.Driver;

namespace GraphQLApplication.Models;

public class PersonResolver
{
    public Task<Person> ResolveAsync(
        [Service] IMongoCollection<Person> collection,
        [ID] Guid id)
    {
        return collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
}
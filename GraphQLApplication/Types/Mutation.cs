using AutoMapper;
using Greet;
using MongoDB.Driver;
using Address = GraphQLApplication.Models.Address;
using CreatePersonInput = GraphQLApplication.Models.CreatePersonInput;
using Person = GraphQLApplication.Models.Person;

namespace GraphQLApplication.Types;

public class Mutation
{
    private readonly IMapper _mapper;

    public Mutation()
    {
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreatePersonInput, Greet.CreatePersonInput>();
            cfg.CreateMap<Address, Greet.Address>();
        }).CreateMapper();
    }

    public async Task<Person> CreatePersonAsync(
        [Service] IMongoCollection<Person> collection,
        [Service] Greeter.GreeterClient client,
        CreatePersonInput input)
    {
        var result = await client.CreatePersonAsync(_mapper.Map<CreatePersonInput, Greet.CreatePersonInput>(input));

        var createdPersonId = Guid.Parse(result.Id);
        var cursor = await collection.FindAsync(p => p.Id == createdPersonId);
        var person = await cursor.FirstOrDefaultAsync();
        return person ?? new Person
        {
            Id = createdPersonId
        };
    }
}
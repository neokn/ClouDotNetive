using AutoMapper;
using Greet;
using Grpc.Core;
using GrpcService.Models;
using MongoDB.Driver;
using Address = Greet.Address;
using CreatePersonInput = Greet.CreatePersonInput;
using Person = GrpcService.Models.Person;

namespace GrpcService.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly IMongoCollection<Person> _collection;
    private readonly ILogger<GreeterService> _logger;
    private readonly IMapper _mapper;
    private readonly PersonContext _personContext;

    public GreeterService(ILogger<GreeterService> logger, PersonContext personContext,
        IMongoCollection<Person> collection)
    {
        _logger = logger;
        _personContext = personContext;
        _collection = collection;
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreatePersonInput, Person>();
            cfg.CreateMap<Address, Models.Address>()
                .ForMember(a => a.Id,
                    opt => opt.Ignore());
        }).CreateMapper();
    }

    public override async Task<Greet.Person> CreatePerson(CreatePersonInput request, ServerCallContext context)
    {
        var person = await _personContext.CreatePersonAsync(_mapper.Map<Person>(request));

        await _collection.InsertOneAsync(person);

        return new Greet.Person
        {
            Id = person.Id.ToString()
        };
    }
}
using Microsoft.EntityFrameworkCore;

namespace GrpcService.Models;

public class PersonContext : DbContext
{
    public PersonContext(DbContextOptions<PersonContext> options) : base(options)
    {
    }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public required DbSet<Person> Persons { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Address>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<Person>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
    }

    public async Task<Person> CreatePersonAsync(Person person)
    {
        Persons.Add(person);
        await SaveChangesAsync();
        return person;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=mssql.k3s.svc.cluster.local,31433;Database=test;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True;");
    }
}
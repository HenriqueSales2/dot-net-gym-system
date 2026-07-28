using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<GymSystemDb> (opt => opt.UseInMemoryDatabase("GymSystemList"));
        builder.Services.AddOpenApi();
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        var endpointPerson = app.MapGroup("/person");

        endpointPerson.MapGet("/", FindAll);
        endpointPerson.MapGet("/{id}", FindById);
        endpointPerson.MapPost("/", Create);
        endpointPerson.MapPut("/{id}", Update);
        endpointPerson.MapPatch("/{id}", Patch);
        endpointPerson.MapDelete("/{id}", Delete);

        app.Run();
    }

    private static async Task<IResult> FindAll(GymSystemDb db)
    {
        return TypedResults.Ok(await db.People.Select(x => new PersonDTO(x)).ToArrayAsync());
    }

    private static async Task<IResult> FindById(long id, GymSystemDb db)
    {
        return await db.People.FindAsync(id)
            is Person person
                ? TypedResults.Ok(new PersonDTO(person))
                : TypedResults.NotFound();
    }

    private static async Task<IResult> Create(PersonDTO personDTO, GymSystemDb db)
    {
        var person = new Person
        {
            FirstName = personDTO.FirstName,
            LastName = personDTO.LastName,
            Address = personDTO.Address,
            Gender = personDTO.Gender,
            Secret = personDTO.Secret,
            IsEnabled = personDTO.IsEnabled
        };

        db.People.Add(person);
        await db.SaveChangesAsync();

        personDTO = new PersonDTO(person);

        return TypedResults.Created($"/person/{personDTO.Id}", personDTO);
    }

    private static async Task<IResult> Update(long id, PersonDTO newPersonDTO, GymSystemDb db)
    {
        var person = await db.People.FindAsync(id);

        if (person is null) return TypedResults.NotFound();

        person.FirstName = newPersonDTO.FirstName;
        person.LastName = newPersonDTO.LastName;
        person.Address = newPersonDTO.Address;
        person.Gender = newPersonDTO.Gender;
        person.Secret = newPersonDTO.Secret;
        person.IsEnabled = newPersonDTO.IsEnabled;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    private static async Task<IResult> Patch(long id, PersonPatchDTO patchDTO, GymSystemDb db)
    {
        var person = await db.People.FindAsync(id);

        if (person is null) return TypedResults.NotFound();

        if (patchDTO.FirstName is not null) person.FirstName = patchDTO.FirstName;
        if (patchDTO.LastName is not null) person.LastName = patchDTO.LastName;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    private static async Task<IResult> Delete(long id, GymSystemDb db)
    {
        if (await db.People.FindAsync(id) is Person person)
        {
            db.People.Remove(person);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }
        return TypedResults.NotFound();
    }
}
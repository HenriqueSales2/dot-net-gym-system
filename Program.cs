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

        app.MapGet("/", async (GymSystemDb db) => 
            await db.People.ToListAsync());

        app.MapGet("/{id}", async (long id, GymSystemDb db) =>
            await db.People.FindAsync(id)
            is Person person
                ? Results.Ok(person)
                : Results.NotFound());

        app.MapPost("/", async (Person person, GymSystemDb db) =>
        {
            db.People.Add(person);
            await db.SaveChangesAsync();

            return Results.Created($"/person/{person.Id}", person);
        });

        app.MapPut("/{id}", async (long id, Person newPerson, GymSystemDb db) =>
        {
            var person = await db.People.FindAsync(id);

            if (person is null) return Results.NotFound();

            person.FirstName = newPerson.FirstName;
            person.LastName = newPerson.LastName;
            person.Address = newPerson.Address;
            person.Gender = newPerson.Gender;
            person.Secret = newPerson.Secret;
            person.IsEnabled = newPerson.IsEnabled;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapPatch("/{id}", async (long id, PersonPatchDTO patch, GymSystemDb db) =>
        {
            var person = await db.People.FindAsync(id);

            if (person is null) return Results.NotFound();

            if (patch.FirstName is not null) person.FirstName = patch.FirstName;
            if (patch.LastName is not null) person.LastName = patch.LastName;
            if (patch.IsEnabled is not null) person.IsEnabled = patch.IsEnabled.Value;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/{id}", async (long id, GymSystemDb db) =>
        {
            if (await db.People.FindAsync(id) is Person person)
            {
                db.People.Remove(person);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }
            return Results.NotFound();
        });
        app.Run();
    }
}
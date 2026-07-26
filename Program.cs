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

        app.MapGet("/person", async (GymSystemDb db) => 
            await db.People.ToListAsync());

        app.MapGet("/person/{id}", async (long id, GymSystemDb db) =>
            await db.People.FindAsync(id)
            is Person person
                ? Results.Ok(person)
                : Results.NotFound());    

        app.MapPost("/person", async (Person person, GymSystemDb db) =>
        {
            db.People.Add(person);
            await db.SaveChangesAsync();

            return Results.Created($"/person/{person.Id}", person);
        });

        app.MapPut("/person/{id}", async (long id, Person newPerson, GymSystemDb db) =>
        {
            var person = await db.People.FindAsync(id);

            if (person is null) return Results.NotFound();

            person.FirstName = newPerson.FirstName;
            person.LastName = newPerson.LastName;
            person.Address = newPerson.Address;
            person.Gender = newPerson.Gender;
            person.Secret = newPerson.Secret;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/person/{id}", async (long id, GymSystemDb db) =>
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
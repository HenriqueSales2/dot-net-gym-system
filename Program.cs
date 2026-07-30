using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;

[assembly : ApiController]

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<GymSystemDb>(options =>
        options.UseInMemoryDatabase("GymSystemDb"));        

        builder.Services.AddScoped<IPersonService, PersonService>();
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.Run();
    }
}
    /*

/*
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
    */
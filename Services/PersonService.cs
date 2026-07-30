using Microsoft.EntityFrameworkCore;

public class PersonService : IPersonService
{
    private readonly GymSystemDb _db;

    public PersonService(GymSystemDb db)
    {
        _db = db;
    }

    public async Task<IResult> FindAllAsync()
    {
        return TypedResults.Ok(await _db.People.Select(x => new PersonDTO(x)).ToArrayAsync());
    }

    public async Task<IResult> FindById(long id)
    {
        return await _db.People.FindAsync(id)
            is Person person
                ? TypedResults.Ok(new PersonDTO(person))
                : TypedResults.NotFound();
    }

    public async Task<PersonDTO> CreateAsync(PersonDTO personDTO)
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

        _db.People.Add(person);
        await _db.SaveChangesAsync();

        return new PersonDTO(person);
    }
}
using Microsoft.EntityFrameworkCore;

public class PersonService : IPersonService
{
    private readonly GymSystemDb _db;

    public PersonService(GymSystemDb db)
    {
        _db = db;
    }

    public async Task<IEnumerable<PersonDTO>> FindAllAsync()
    {
        return await _db.People.Select(x => new PersonDTO(x)).ToArrayAsync();
    }

    public async Task<Person?> FindByIdAsync(long id)
    {
        return await _db.People.FindAsync(id);
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

     public async Task<bool> UpdateAsync(long id, PersonDTO newPersonDTO)
    {
        var person = await _db.People.FindAsync(id);

        if (person is null || newPersonDTO is null) return false;

        person.FirstName = newPersonDTO.FirstName;
        person.LastName = newPersonDTO.LastName;
        person.Address = newPersonDTO.Address;
        person.Gender = newPersonDTO.Gender;
        person.Secret = newPersonDTO.Secret;
        person.IsEnabled = newPersonDTO.IsEnabled;

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> PatchAsync(long id, PersonPatchDTO patchDTO)
    {
        var person = await _db.People.FindAsync(id);

        if (person is null || patchDTO is null) return false;

        if (patchDTO.FirstName is not null) person.FirstName = patchDTO.FirstName;
        if (patchDTO.LastName is not null) person.LastName = patchDTO.LastName;
        if (patchDTO.IsEnabled is not null) person.IsEnabled = patchDTO.IsEnabled;

        await _db.SaveChangesAsync();

        return true;
    }

     public async Task<bool> DeleteAsync(long id)
    {
        var person = await _db.People.FindAsync(id);
        
        if (person == null)
        {
            return false;
        }
            _db.People.Remove(person);
            await _db.SaveChangesAsync();
            return true;
        }
    }
public interface IPersonService
{
    Task<IEnumerable<PersonDTO>> FindAllAsync();
    Task<Person?> FindByIdAsync(long id);
    Task<PersonDTO> CreateAsync(PersonDTO personDTO);
    Task<bool> UpdateAsync(long id, PersonDTO newPersonDTO);
    Task<bool> PatchAsync(long id, PersonPatchDTO patchDTO);
    Task<bool> DeleteAsync(long id);
}
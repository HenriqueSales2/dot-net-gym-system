public interface IPersonService
{
    Task<PersonDTO> CreateAsync(PersonDTO personDTO);
    Task<IResult> FindAllAsync();
    Task<IResult> FindById(long id);
}
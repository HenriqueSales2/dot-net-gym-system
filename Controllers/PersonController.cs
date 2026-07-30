using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _service;

    public PersonController (IPersonService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PersonDTO>>> FindAll()
    {
        var people = await _service.FindAllAsync();

        return Ok(people);
    } 

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonDTO>> FindById(long id)
    {
        var person = await _service.FindByIdAsync(id);

        return person is not null
            ? Ok(new PersonDTO(person))
            : NotFound();
    } 

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PersonDTO>> Create(PersonDTO personDTO)
    {
        var user = await _service.CreateAsync(personDTO);

        return Created($"/person/{user.Id}",user);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(long id, PersonDTO personDTO)
    {
        var isUpdate = await _service.UpdateAsync(id, personDTO);

        if (!isUpdate)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Patch(long id, PersonPatchDTO patchDTO)
    {
        var isPatch = await _service.PatchAsync(id, patchDTO);

        if (!isPatch)
        {
            return NotFound();
        }
        return NoContent();
    } 

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(long id)
    {
        var isDeleted = await _service.DeleteAsync(id);

        if (!isDeleted)
        {
            return NotFound();
        }
        return NoContent();
    } 
}
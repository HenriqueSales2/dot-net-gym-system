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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PersonDTO>> FindAll()
    {
        var user = await _service.FindAllAsync();

        return Ok(user);
    } 

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PersonDTO>> FindById(long id)
    {
        var user = await _service.FindById(id);
        
        return Ok(user);
    } 



    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PersonDTO>> Create(PersonDTO personDTO)
    {
        var user = await _service.CreateAsync(personDTO);

        return Created($"/person/{user.Id}",user);
    } 
}
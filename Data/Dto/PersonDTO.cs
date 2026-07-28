public class PersonDTO
{
    public long Id { get; set; }
    public string? FirstName { get; set; }
    public  string? LastName { get; set; }
    public string? Address { get; set; }
    public string? Gender { get; set; }
    public string? Secret { get; set; }
    public bool? IsEnabled { get; set; }

    public PersonDTO() { }
    public PersonDTO(Person person) =>
    (Id, 
    FirstName, LastName, Address, Gender, Secret, IsEnabled) = 
    (person.Id, person.FirstName, person.LastName, person.Address, person.Gender, person.Secret, person.IsEnabled);
}
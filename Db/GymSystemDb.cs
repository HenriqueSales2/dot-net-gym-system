using Microsoft.EntityFrameworkCore;

public class GymSystemDb : DbContext
{
    public GymSystemDb(DbContextOptions<GymSystemDb> options)
        : base(options) { }

    public DbSet<Person> People => Set<Person>();    
}
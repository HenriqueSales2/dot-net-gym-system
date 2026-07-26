using Microsoft.EntityFrameworkCore;

class GymSystemDb : DbContext
{
    public GymSystemDb(DbContextOptions<GymSystemDb> options)
        : base(options) { }

    public DbSet<Person> People => Set<Person>();    
}
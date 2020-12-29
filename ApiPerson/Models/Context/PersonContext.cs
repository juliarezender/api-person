using Microsoft.EntityFrameworkCore;

namespace ApiPerson.Models.Context
{
    public class PersonContext
    {
        public PersonContext(DbContextOptions<PersonContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
}
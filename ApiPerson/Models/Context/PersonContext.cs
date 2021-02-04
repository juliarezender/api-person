using Microsoft.EntityFrameworkCore;

namespace ApiPerson.Models.Context
{
    public class PersonContext : DbContext
    {
        public PersonContext(DbContextOptions<PersonContext> options) : base(options)
        {
        }
        public PersonContext() : base()
        {
        }

        public virtual DbSet<Person> Persons { get; set; }
    }
}
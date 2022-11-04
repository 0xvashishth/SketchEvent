using EventoWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace EventoWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }

    }
}

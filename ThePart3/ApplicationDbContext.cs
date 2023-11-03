using Microsoft.EntityFrameworkCore;
using ThePart3.Models;

namespace ThePart3
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Semesters> Semesters { get; set; }
        public DbSet<Modules> Modules { get; set; }
    }
}

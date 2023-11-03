using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StudyWithMe
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleCalendar> ModuleCalendars { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

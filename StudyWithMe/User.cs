using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyWithMe
{
    public class User
    {
        [Key]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        [Required] 
        public string name { get; set; }
        [Required]
        public string surname { get; set; }
        [Required]
        public string email { get; set; }
        public ICollection<Module> Modules { get; set; }
        public ICollection<Semester> Semesters { get; set; }
    }
}

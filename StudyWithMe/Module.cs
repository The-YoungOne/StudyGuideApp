using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyWithMe
{
    public class Module
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int module_id { get; set; }
        [Required]
        public string code { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public int credits { get; set; }
        [Required]
        public int classHrsPerWeek { get; set; }
        public User Username { get; set; }
        public ICollection<ModuleCalendar> ModuleCalendars { get; set; }
    }
}

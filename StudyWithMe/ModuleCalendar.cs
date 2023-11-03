using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyWithMe
{
    public class ModuleCalendar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int calendar_id { get; set; }
        public DateTime studyDate { get; set; }
        public double hoursStudied { get; set; }
        public int ModuleId { get; set; }
        public Module Module { get; set; }
    }
}

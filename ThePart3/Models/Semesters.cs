using System.ComponentModel.DataAnnotations;

namespace ThePart3.Models
{
    public class Semesters
    {
        [Key]
        public int semesterId { get; set; }
        public int weeks { get; set; }
        public DateTime startDate { get; set; }
    }
}

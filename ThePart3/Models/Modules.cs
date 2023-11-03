using System.ComponentModel.DataAnnotations;

namespace ThePart3.Models
{
    public class Modules
    {
        [Key]
        public int module_id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int credits { get; set; }
        public int classHrsPerWeek { get; set; }
    }
}

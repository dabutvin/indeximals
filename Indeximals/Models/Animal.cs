using System.ComponentModel.DataAnnotations;

namespace Indeximals.Models
{
    public class Animal
    {
        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Sound")]
        public string Sound { get; set; }

        [Required]
        [Display(Name = "Diet")]
        public Diet Diet { get; set; }
    }
}

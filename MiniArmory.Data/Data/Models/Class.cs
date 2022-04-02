using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Data.Data.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(500)]
        public string ClassImage { get; set; }

        [Required]
        [StringLength(20)]
        public string SpecialisationName { get; set; }

        [Required]
        [StringLength(500)]
        public string SpecialisationDescription { get; set; }

        [Required]
        [StringLength(500)]
        public string SpecialisationImage { get; set; }

        [Required]
        [StringLength(500)]
        public string Image { get; set; }

        public ICollection<Character> Characters { get; set; } = new List<Character>();

        public ICollection<Spell> Spells { get; set; } = new List<Spell>();
    }
}

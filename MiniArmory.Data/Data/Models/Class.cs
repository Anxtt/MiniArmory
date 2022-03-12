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
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [StringLength(200)]
        public string SpecialisationDescription { get; set; }

        [Required]
        [StringLength(200)]
        public string SpecialisationImage { get; set; }

        public ICollection<Character> Characters { get; set; } = new List<Character>();

        public ICollection<Spell> Spells { get; set; } = new List<Spell>();
    }
}

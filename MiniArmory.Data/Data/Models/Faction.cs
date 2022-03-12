using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Data.Data.Models
{
    public class Faction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        [StringLength(2000)]
        public string Image { get; set; }

        public ICollection<Race> Races { get; set; } = new List<Race>();

        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}

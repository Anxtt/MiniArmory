using System.ComponentModel.DataAnnotations;

using static MiniArmory.GlobalConstants.Data;

namespace MiniArmory.Data.Models
{
    public class Faction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(FactionConst.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        [StringLength(FactionConst.DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        [Required]
        [StringLength(IMAGE_MAX_LENGTH)]
        public string Image { get; set; }

        public ICollection<Race> Races { get; set; } = new List<Race>();

        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}

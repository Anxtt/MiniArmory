using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static MiniArmory.GlobalConstants.Data;

namespace MiniArmory.Data.Data.Models
{
    public class Race
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(RaceConst.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        [StringLength(RaceConst.DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        [Required]
        [StringLength(IMAGE_MAX_LENGTH)]
        public string Image { get; set; }

        [ForeignKey(nameof(RacialSpell))]
        public int SpellId { get; set; }

        public Spell RacialSpell { get; set; }

        [Required]
        [ForeignKey(nameof(Faction))]
        public int FactionId { get; set; }

        public Faction Faction { get; set; }

        public ICollection<Character> Characters { get; set; }

        [Required]
        [StringLength(RaceConst.ARMS_MAX_LENGTH)]
        public string Arms { get; set; }
    }
}

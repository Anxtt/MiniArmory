using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniArmory.Data.Data.Models
{
    public class Race
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [ForeignKey(nameof(RacialSpell))]
        public int SpellId { get; set; }

        public Spell RacialSpell { get; set; }

        [Required]
        [ForeignKey(nameof(Faction))]
        public int FactionId { get; set; }

        public Faction Faction { get; set; }
    }
}

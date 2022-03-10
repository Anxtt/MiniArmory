using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [Required]
        [ForeignKey(nameof(RacialSpell))]
        public int SpellId { get; set; }

        public Spell RacialSpell { get; set; }

        [Required]
        [ForeignKey(nameof(Faction))]
        public int FactionId { get; set; }

        public Faction Faction { get; set; }

        public ICollection<Class> Classes { get; set; } = new List<Class>();

        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}

using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Race
{
    public class RaceFormModel
    {
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Racial Spell")]
        [RegularExpression("^[\"\\d\"]+$", ErrorMessage = "You must select a racial spell.")]
        public string RacialSpell { get; set; }

        [Required]
        [RegularExpression("^[\"\\d\"]+$", ErrorMessage = "You must select a faction.")]
        public string Faction { get; set; }
    }
}

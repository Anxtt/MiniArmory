using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Race
{
    public class RaceFormModel
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 50)]
        public string Description { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 30)]
        public string Image { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 30)]
        public string Arms { get; set; }

        [Required]
        [Display(Name = "Racial Spell")]
        [RegularExpression("^[\"\\d\"]+$", ErrorMessage = "You must select a racial spell.")]
        public string RacialSpell { get; set; }

        [Required]
        [RegularExpression("^[\"\\d\"]+$", ErrorMessage = "You must select a faction.")]
        public string Faction { get; set; }
    }
}

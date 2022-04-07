using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Race
{
    public class RaceFormModel
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Must have a name between 3 and 20 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 50, ErrorMessage = "Must have a description between 50 and 200 characters.")]
        public string Description { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 30, ErrorMessage = "Must have an image. Please post a link.")]
        public string Image { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 30, ErrorMessage = "Must have an image.Please post a link.")]
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

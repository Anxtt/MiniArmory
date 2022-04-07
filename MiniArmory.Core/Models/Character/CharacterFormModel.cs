using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Character
{
    public class CharacterFormModel
    {
        [Required]
        [StringLength(16, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[\"\\d\"]+$", ErrorMessage = "You must select an option different than the default one.")]
        public int Realm { get; set; }

        [Required]
        [RegularExpression("^[\"\\d\"]+$", ErrorMessage = "You must select an option different than the default one.")]
        public int Faction { get; set; }

        [Required]
        [RegularExpression("^[\"\\d\"]+$", ErrorMessage = "You must select an option different than the default one.")]
        public int Race { get; set; }

        [Required]
        [RegularExpression("^[\"\\d\"]+$", ErrorMessage = "You must select an option different than the default one.")]
        public int Class { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 30)]
        public string Image { get; set; }
    }
}

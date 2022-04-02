using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Character
{
    public class CharacterFormModel
    {
        [Required]
        [StringLength(16)]
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
        [StringLength(500)]
        public string Image { get; set; }
    }
}

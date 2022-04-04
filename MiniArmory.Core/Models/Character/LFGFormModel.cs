using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Character
{
    public class LFGFormModel : CharacterViewModel
    {
        [Required]
        [RegularExpression("^[\"\\d\"]+$", ErrorMessage = "You must select a class.")]
        public string Class { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        public short MinRating { get; set; }
    }
}

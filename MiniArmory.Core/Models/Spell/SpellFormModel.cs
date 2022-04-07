using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Spell
{
    public class SpellFormModel
    {
        [Required]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Must have a name between 4 and 30 characters.")]
        public string Name { get; set; }

        [Required]
        [Range(5, 40, ErrorMessage = "Must have a range between 5 and 40 yards.")]
        public sbyte Range { get; set; }

        [Required]
        [Range(0, 600, ErrorMessage = "Must have cooldown between 0 and 600 seconds.")]
        public short Cooldown { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 30, ErrorMessage = "Must have an image. Please post a link.")]
        public string Tooltip { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 50, ErrorMessage = "Must have a description between 50 and 200 characters.")]
        public string Description { get; set; }

        [Required]
        [RegularExpression("^[A-Z{1}a-z]+$", ErrorMessage = "The type must be different than the default option.")]
        public string Type { get; set; }

        [RegularExpression("^[\"\\d\"]+$", ErrorMessage = "You must select a class.")]
        public string? Class { get; set; }

        public string? Race { get; set; }
    }
}

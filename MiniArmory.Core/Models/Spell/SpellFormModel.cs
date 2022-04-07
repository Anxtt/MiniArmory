using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Spell
{
    public class SpellFormModel
    {
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        [Range(5, 40)]
        public sbyte Range { get; set; }

        [Required]
        [Range(0, 600)]
        public short Cooldown { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 30)]
        public string Tooltip { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 50)]
        public string Description { get; set; }

        [Required]
        [RegularExpression("^[A-Z{1}a-z]+$", ErrorMessage = "The type must be different than the default option.")]
        public string Type { get; set; }

        [RegularExpression("^[\"\\d\"]+$", ErrorMessage = "You must select a class.")]
        public string? Class { get; set; }

        public string? Race { get; set; }
    }
}

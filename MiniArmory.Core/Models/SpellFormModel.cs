using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models
{
    public class SpellFormModel
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [Range(5, 40)]
        public sbyte Range { get; set; }

        [Required]
        [Range(0, 600)]
        public short Cooldown { get; set; }

        [Required]
        [StringLength(200)]
        public string Tooltip { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [MinLength(4)]
        public string Type { get; set; }

        [Required]
        [MinLength(4)]
        public string Class { get; set; }

        [Required]
        public string Race { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Mount
{
    public class MountFormModel
    {
        [Required]
        [StringLength(60, MinimumLength = 10)]
        public string Name { get; set; }

        [Required]
        [Range(80, 100)]
        [Display(Name = "Ground Speed")]
        public sbyte GroundSpeed { get; set; }

        [Required]
        [Range(80, 100)]
        [Display(Name = "Flying Speed")]
        public sbyte FlyingSpeed { get; set; }

        public string Faction { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 30)]
        public string Image { get; set; }
    }
}

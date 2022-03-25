using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models
{
    public class MountFormModel
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [Range(80, 100)]
        public sbyte GroundSpeed { get; set; }

        [Required]
        [Range(80, 100)]
        public sbyte FlyingSpeed { get; set; }

        public string Faction { get; set; }

        [Required]
        [StringLength(200)]
        public string Image { get; set; }
    }
}

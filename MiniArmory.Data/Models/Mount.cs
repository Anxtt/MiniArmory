using System.ComponentModel.DataAnnotations;

using static MiniArmory.GlobalConstants.Data;

namespace MiniArmory.Data.Models
{
    public class Mount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(MountConst.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        [Range(MountConst.SPEED_MIN, MountConst.SPEED_MAX)]
        public sbyte GroundSpeed { get; set; }

        [Required]
        [Range(MountConst.SPEED_MIN, MountConst.SPEED_MAX)]
        public sbyte FlyingSpeed { get; set; }

        [Required]
        [StringLength(IMAGE_MAX_LENGTH)]
        public string Image { get; set; }

        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}

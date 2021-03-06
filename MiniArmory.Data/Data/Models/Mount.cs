using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Data.Data.Models
{
    public class Mount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        [Required]
        [Range(80, 100)]
        public sbyte GroundSpeed { get; set; }

        [Required]
        [Range(80, 100)]
        public sbyte FlyingSpeed { get; set; }

        [Required]
        [StringLength(200)]
        public string Image { get; set; }

        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}

using System.ComponentModel.DataAnnotations;
using MiniArmory.Data.Data.Models.Enums;

namespace MiniArmory.Data.Data.Models
{
    public class Mount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public GroundMountSpeed GroundSpeed { get; set; }
    
        [Required]
        public bool CanFly { get; set; }

        public FlyingMountSpeed FlyingSpeed { get; set; }

        public string Faction { get; set; }

        [Required]
        [StringLength(200)]
        public string Image { get; set; }

        [Required]
        public bool IsCollected { get; set; }

        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}

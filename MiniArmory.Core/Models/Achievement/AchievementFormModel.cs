using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Achievement
{
    public class AchievementFormModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(5, 50)]
        public byte Points { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [StringLength(30)]
        public string Category { get; set; }

        [Required]
        [StringLength(500)]
        public string Image { get; set; }
    }
}

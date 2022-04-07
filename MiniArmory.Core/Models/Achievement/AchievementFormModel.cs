using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Achievement
{
    public class AchievementFormModel
    {
        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Name { get; set; }

        [Required]
        [Range(5, 50)]
        public byte Points { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 20)]
        public string Description { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Category { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 30)]
        public string Image { get; set; }
    }
}

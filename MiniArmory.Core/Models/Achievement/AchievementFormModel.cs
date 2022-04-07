using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Achievement
{
    public class AchievementFormModel
    {
        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Must have a name between 6 and 50 characters.")]
        public string Name { get; set; }

        [Required]
        [Range(5, 50, ErrorMessage = "Must have an amount of points between 5 and 50.")]
        public byte Points { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 20, ErrorMessage = "Must have a description between 20 and 100 characters.")]
        public string Description { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Must have a category between 3 and 30 characters.")]
        public string Category { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 30, ErrorMessage = "Must have an image. Please post a link.")]
        public string Image { get; set; }
    }
}

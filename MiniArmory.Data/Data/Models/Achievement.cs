using System.ComponentModel.DataAnnotations;

using static MiniArmory.GlobalConstants.Data;

namespace MiniArmory.Data.Data.Models
{
    public class Achievement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(AchievementConst.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        [Range(AchievementConst.POINTS_MIN, AchievementConst.POINTS_MAX)]
        public byte Points { get; set; }

        [Required]
        [StringLength(AchievementConst.DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        [Required]
        [StringLength(AchievementConst.CATEGORY_MAX_LENGTH)]
        public string Category { get; set; }

        [Required]
        [StringLength(IMAGE_MAX_LENGTH)]
        public string Image { get; set; }

        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}

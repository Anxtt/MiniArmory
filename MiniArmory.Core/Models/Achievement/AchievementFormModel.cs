using System.ComponentModel.DataAnnotations;

using static MiniArmory.GlobalConstants.Data;
using static MiniArmory.GlobalConstants.ErrorMessage;

namespace MiniArmory.Core.Models.Achievement
{
    public class AchievementFormModel
    {
        [Required]
        [StringLength(
            AchievementConst.NAME_MAX_LENGTH,
            MinimumLength = AchievementConst.NAME_MIN_LENGTH,
            ErrorMessage = TEXT_FIELD)]
        public string Name { get; set; }

        [Required]
        [Range(
            AchievementConst.POINTS_MIN,
            AchievementConst.POINTS_MAX,
            ErrorMessage = NUMBERS_FIELD)]
        public byte Points { get; set; }

        [Required]
        [StringLength(
            AchievementConst.DESCRIPTION_MAX_LENGTH,
            MinimumLength = AchievementConst.DESCRIPTION_MIN_LENGTH,
            ErrorMessage = TEXT_FIELD)]
        public string Description { get; set; }

        [Required]
        [StringLength(
            AchievementConst.CATEGORY_MAX_LENGTH,
            MinimumLength = AchievementConst.CATEGORY_MIN_LENGTH,
            ErrorMessage = TEXT_FIELD)]
        public string Category { get; set; }

        [Required]
        [StringLength(
            IMAGE_MAX_LENGTH,
            MinimumLength = IMAGE_MIN_LENGTH,
            ErrorMessage = IMAGE_GENERAL_MESSAGE)]
        public string Image { get; set; }
    }
}

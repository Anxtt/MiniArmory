using System.ComponentModel.DataAnnotations;

using static MiniArmory.GlobalConstants.Data;
using static MiniArmory.GlobalConstants.ErrorMessage;

namespace MiniArmory.Core.Models.Mount
{
    public class MountFormModel
    {
        [Required]
        [StringLength(MountConst.NAME_MAX_LENGTH,
            MinimumLength = MountConst.NAME_MIN_LENGTH,
            ErrorMessage = TEXT_FIELD)]
        public string Name { get; set; }

        [Required]
        [Range(MountConst.SPEED_MIN,
            MountConst.SPEED_MAX,
            ErrorMessage = NUMBERS_FIELD)]
        [Display(Name = MountConst.GROUND_SPEED)]
        public sbyte GroundSpeed { get; set; }

        [Required]
        [Range(MountConst.SPEED_MIN,
            MountConst.SPEED_MAX,
            ErrorMessage = NUMBERS_FIELD)]
        [Display(Name = MountConst.FLYING_SPEED)]
        public sbyte FlyingSpeed { get; set; }

        [Required]
        [StringLength(IMAGE_MAX_LENGTH,
            MinimumLength = IMAGE_MIN_LENGTH,
            ErrorMessage = IMAGE_GENERAL_MESSAGE)]
        [RegularExpression(IMAGE_REGEX,
            ErrorMessage = INVALID_IMAGE)]
        public string Image { get; set; }
    }
}

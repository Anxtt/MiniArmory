using System.ComponentModel.DataAnnotations;

using static MiniArmory.GlobalConstants.Data;
using static MiniArmory.GlobalConstants.ErrorMessage;

namespace MiniArmory.Core.Models.Race
{
    public class RaceFormModel
    {
        [Required]
        [StringLength(RaceConst.NAME_MAX_LENGTH,
            MinimumLength = RaceConst.NAME_MIN_LENGTH,
            ErrorMessage = TEXT_FIELD)]
        public string Name { get; set; }

        [Required]
        [StringLength(RaceConst.DESCRIPTION_MAX_LENGTH,
            MinimumLength = RaceConst.DESCRIPTION_MIN_LENGTH,
            ErrorMessage = TEXT_FIELD)]
        public string Description { get; set; }

        [Required]
        [StringLength(IMAGE_MAX_LENGTH,
            MinimumLength = IMAGE_MIN_LENGTH,
            ErrorMessage = IMAGE_GENERAL_MESSAGE)]
        [RegularExpression(IMAGE_REGEX,
            ErrorMessage = INVALID_IMAGE)]
        public string Image { get; set; }

        [Required]
        [StringLength(IMAGE_MAX_LENGTH,
            MinimumLength = IMAGE_MIN_LENGTH,
            ErrorMessage = IMAGE_GENERAL_MESSAGE)]
        [RegularExpression(IMAGE_REGEX,
            ErrorMessage = INVALID_IMAGE)]
        public string Arms { get; set; }

        [Required]
        [Display(Name = RaceConst.RACIAL_SPELL)]
        [RegularExpression(TEXT_FIELD_REGEX, ErrorMessage = SELECT)]
        public string RacialSpell { get; set; }

        [Required]
        [RegularExpression(TEXT_FIELD_REGEX, ErrorMessage = SELECT)]
        public string Faction { get; set; }
    }
}

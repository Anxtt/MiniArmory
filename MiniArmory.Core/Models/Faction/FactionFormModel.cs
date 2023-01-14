using System.ComponentModel.DataAnnotations;

using static MiniArmory.GlobalConstants.Data;
using static MiniArmory.GlobalConstants.ErrorMessage;

namespace MiniArmory.Core.Models.Faction
{
    public class FactionFormModel
    {
        [Required]
        [StringLength(FactionConst.NAME_MAX_LENGTH,
            MinimumLength = FactionConst.NAME_MIN_LENGTH,
            ErrorMessage = TEXT_FIELD)]
        public string Name { get; set; }

        [Required]
        [StringLength(FactionConst.DESCRIPTION_MAX_LENGTH,
            MinimumLength = FactionConst.DESCRIPTION_MIN_LENGTH, 
            ErrorMessage = TEXT_FIELD)]
        public string Description { get; set; }

        [Required]
        [StringLength(IMAGE_MAX_LENGTH,
            MinimumLength = IMAGE_MIN_LENGTH,
            ErrorMessage = IMAGE_GENERAL_MESSAGE)]
        [RegularExpression(IMAGE_REGEX,
            ErrorMessage = INVALID_IMAGE)]
        public string Image { get; set; }
    }
}

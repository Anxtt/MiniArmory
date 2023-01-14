using System.ComponentModel.DataAnnotations;

using static MiniArmory.GlobalConstants.Data;
using static MiniArmory.GlobalConstants.ErrorMessage;

namespace MiniArmory.Core.Models.Character
{
    public class CharacterFormModel
    {
        [Required]
        [StringLength(CharacterConst.NAME_MAX_LENGTH,
            MinimumLength = CharacterConst.NAME_MIN_LENGTH,
            ErrorMessage = TEXT_FIELD)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(TEXT_FIELD_REGEX, ErrorMessage = DROPDOWN)]
        public int Realm { get; set; }

        [Required]
        [RegularExpression(TEXT_FIELD_REGEX, ErrorMessage = DROPDOWN)]
        public int Faction { get; set; }

        [Required]
        [RegularExpression(TEXT_FIELD_REGEX, ErrorMessage = DROPDOWN)]
        public int Race { get; set; }

        [Required]
        [RegularExpression(TEXT_FIELD_REGEX, ErrorMessage = DROPDOWN)]
        public int Class { get; set; }

        [Required]
        [StringLength(IMAGE_MAX_LENGTH,
            MinimumLength = IMAGE_MIN_LENGTH,
            ErrorMessage = IMAGE_GENERAL_MESSAGE)]
        [RegularExpression(IMAGE_REGEX, 
            ErrorMessage = INVALID_IMAGE)]
        public string Image { get; set; }
    }
}

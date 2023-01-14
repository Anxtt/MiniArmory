using System.ComponentModel.DataAnnotations;

using static MiniArmory.GlobalConstants.Data;
using static MiniArmory.GlobalConstants.ErrorMessage;

namespace MiniArmory.Core.Models.Spell
{
    public class SpellFormModel
    {
        [Required]
        [StringLength(
            SpellConst.NAME_MAX_LENGTH,
            MinimumLength = SpellConst.NAME_MIN_LENGTH,
            ErrorMessage = TEXT_FIELD)]
        public string Name { get; set; }

        [Required]
        [Range(SpellConst.RANGE_MIN,
            SpellConst.RANGE_MAX,
            ErrorMessage = RANGE_FIELD)]
        public sbyte Range { get; set; }

        [Required]
        [Range(SpellConst.COOLDOWN_MIN,
            SpellConst.COOLDOWN_MAX,
            ErrorMessage = COOLDOWN_FIELD)]
        public short Cooldown { get; set; }

        [Required]
        [StringLength(IMAGE_MAX_LENGTH,
            MinimumLength = IMAGE_MIN_LENGTH,
            ErrorMessage = IMAGE_GENERAL_MESSAGE)]
        [RegularExpression(IMAGE_REGEX,
            ErrorMessage = INVALID_IMAGE)]
        public string Tooltip { get; set; }

        [Required]
        [StringLength(SpellConst.DESCRIPTION_MAX_LENGTH,
            MinimumLength = SpellConst.DESCRIPTION_MIN_LENGTH,
            ErrorMessage = TEXT_FIELD)]
        public string Description { get; set; }

        [Required]
        [RegularExpression(TYPE_REGEX, ErrorMessage = DROPDOWN)]
        public string Type { get; set; }

        [RegularExpression(TEXT_FIELD_REGEX, ErrorMessage = SELECT)]
        public string? Class { get; set; }

        [RegularExpression(TEXT_FIELD_REGEX, ErrorMessage = SELECT)]
        public string? Race { get; set; }
    }
}

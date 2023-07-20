using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

using MiniArmory.Core.Attributes;

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
        [DataType(DataType.Upload)]
        [AllowedExtensions()]
        public IFormFile Image { get; set; }
    }
}

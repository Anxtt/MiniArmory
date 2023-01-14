using System.ComponentModel.DataAnnotations;

using static MiniArmory.GlobalConstants.Data;
using static MiniArmory.GlobalConstants.ErrorMessage;

namespace MiniArmory.Core.Models.Realm
{
    public class RealmFormModel
    {
        [Required]
        [StringLength(RealmConst.NAME_MAX_LENGTH,
            MinimumLength = RealmConst.NAME_MIN_LENGTH,
            ErrorMessage = TEXT_FIELD)]
        public string Name { get; set; }

        [Required]
        [StringLength(RealmConst.LANGUAGE_MAX_LENGTH,
            MinimumLength = RealmConst.LANGUAGE_MIN_LENGTH,
            ErrorMessage = TEXT_FIELD)]
        public string Language { get; set; }
    }
}

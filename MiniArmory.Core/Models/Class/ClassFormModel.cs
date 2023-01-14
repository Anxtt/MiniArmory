using System.ComponentModel.DataAnnotations;

using static MiniArmory.GlobalConstants.Data;
using static MiniArmory.GlobalConstants.ErrorMessage;

namespace MiniArmory.Core.Models.Class
{
    public class ClassFormModel
    {
        [Required]
        [StringLength(ClassConst.NAME_MAX_LENGTH,
            MinimumLength = ClassConst.NAME_MIN_LENGTH,
            ErrorMessage = TEXT_FIELD)]
        public string Name { get; set; }

        [Required]
        [StringLength(ClassConst.DESCRIPTION_MAX_LENGTH,
            MinimumLength = ClassConst.DESCRIPTION_MIN_LENGTH,
            ErrorMessage = TEXT_FIELD)]
        public string Description { get; set; }

        [Required]
        [StringLength(IMAGE_MAX_LENGTH,
            MinimumLength = IMAGE_MIN_LENGTH,
            ErrorMessage = IMAGE_GENERAL_MESSAGE)]
        [RegularExpression(IMAGE_REGEX,
            ErrorMessage = INVALID_IMAGE)]
        [Display(Name = ClassConst.CLASS_IMAGE)]
        public string ClassImage { get; set; }

        [Required]
        [StringLength(IMAGE_MAX_LENGTH,
            MinimumLength = IMAGE_MIN_LENGTH,
            ErrorMessage = IMAGE_GENERAL_MESSAGE)]
        [RegularExpression(IMAGE_REGEX, 
            ErrorMessage = INVALID_IMAGE)]
        public string Image { get; set; }

        [Required]
        [StringLength(ClassConst.NAME_MAX_LENGTH,
            MinimumLength = ClassConst.NAME_MIN_LENGTH,
            ErrorMessage = TEXT_FIELD)]
        [Display(Name = ClassConst.SPECIALISATION_NAME)]
        public string SpecialisationName { get; set; }

        [Required]
        [StringLength(ClassConst.DESCRIPTION_MAX_LENGTH,
            MinimumLength = ClassConst.DESCRIPTION_MIN_LENGTH,
            ErrorMessage = TEXT_FIELD)]
        [Display(Name = ClassConst.SPECIALISATION_DESCRIPTION)]
        public string SpecialisationDescription { get; set; }

        [Required]
        [StringLength(IMAGE_MAX_LENGTH,
            MinimumLength = IMAGE_MIN_LENGTH,
            ErrorMessage = IMAGE_GENERAL_MESSAGE)]
        [RegularExpression(IMAGE_REGEX,
            ErrorMessage = INVALID_IMAGE)]
        [Display(Name = ClassConst.SPECIALISATION_IMAGE)]
        public string SpecialisationImage { get; set; }
    }
}

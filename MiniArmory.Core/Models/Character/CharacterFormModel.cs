using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Character
{
    public class CharacterFormModel
    {
        [Required]
        [StringLength(16, MinimumLength = 3, ErrorMessage = "Must have a name between 3 and 16 characters.")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[\"\\d\"]+$", ErrorMessage = "You must select an option different than the default one.")]
        public int Realm { get; set; }

        [Required]
        [RegularExpression("^[\"\\d\"]+$", ErrorMessage = "You must select an option different than the default one.")]
        public int Faction { get; set; }

        [Required]
        [RegularExpression("^[\"\\d\"]+$", ErrorMessage = "You must select an option different than the default one.")]
        public int Race { get; set; }

        [Required]
        [RegularExpression("^[\"\\d\"]+$", ErrorMessage = "You must select an option different than the default one.")]
        public int Class { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 30, ErrorMessage = "Must have an image. Please post a link.")]
        [RegularExpression(@"^(https://)(www)?(render-(eu)?)?(render)?(wow)?(assets)?(images)?(imgur)?.?(deviantart)?(com)?(worldofwarcraft)?(blz-contentstack)?(zamimg)?(.)?(/)?(.)?(com)/(images)?/?(wow)?/?(icons)?.*$", 
            ErrorMessage = "Only images from deviantart, wow armory, wowhead, and imgur are allowed.")]
        public string Image { get; set; }
    }
}

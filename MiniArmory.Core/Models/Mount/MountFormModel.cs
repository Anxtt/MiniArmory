using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Mount
{
    public class MountFormModel
    {
        [Required]
        [StringLength(60, MinimumLength = 10, ErrorMessage = "Must have a name between 10 and 60 characters.")]
        public string Name { get; set; }

        [Required]
        [Range(80, 100, ErrorMessage = "Must have an amount of speed between 80 and 100.")]
        [Display(Name = "Ground Speed")]
        public sbyte GroundSpeed { get; set; }

        [Required]
        [Range(80, 100, ErrorMessage = "Must have an amount of speed between 80 and 100.")]
        [Display(Name = "Flying Speed")]
        public sbyte FlyingSpeed { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 30, ErrorMessage = "Must have an image. Please post a link.")]
        [RegularExpression(@"^(https://)(www)?(render-(eu)?)?(render)?(wow)?(assets)?(images)?(imgur)?.?(deviantart)?(com)?(worldofwarcraft)?(blz-contentstack)?(zamimg)?(.)?(/)?(.)?(com)/(images)?/?(wow)?/?(icons)?.*$",
            ErrorMessage = "Only images from DeviantArt, WoW Armory, WoWHead, and Imgur are allowed.")]
        public string Image { get; set; }
    }
}

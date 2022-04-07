using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Faction
{
    public class FactionFormModel
    {
        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Must have a name between 5 and 20 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 50, ErrorMessage = "Must have a description between 50 and 500 characters.")]
        public string Description { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 30, ErrorMessage = "Must have an image. Please post a link.")]
        public string Image { get; set; }
    }
}

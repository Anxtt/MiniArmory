using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Class
{
    public class ClassFormModel
    {
        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Must have a name between 4 and 20 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 50, ErrorMessage = "Must have a description between 50 and 500 characters.")]
        public string Description { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 30, ErrorMessage = "Must have an image. Please post a link.")]
        [Display(Name = "Class Image")]
        public string ClassImage { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 30, ErrorMessage = "Must have an image. Please post a link.")]
        public string Image { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Must have a name between 4 and 20 characters.")]
        [Display(Name = "Specialisation Name")]
        public string SpecialisationName { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 30, ErrorMessage = "Must have a description between 30 and 500 characters.")]
        [Display(Name = "Specialisation Description")]
        public string SpecialisationDescription { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 30, ErrorMessage = "Must have an image. Please post a link.")]
        [Display(Name = "Specialisation Image")]
        public string SpecialisationImage { get; set; }
    }
}

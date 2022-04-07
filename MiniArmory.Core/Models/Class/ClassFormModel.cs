using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Class
{
    public class ClassFormModel
    {
        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 50)]
        public string Description { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 30)]
        [Display(Name = "Class Image")]
        public string ClassImage { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 30)]
        public string Image { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4)]
        [Display(Name = "Specialisation Name")]
        public string SpecialisationName { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 50)]
        [Display(Name = "Specialisation Description")]
        public string SpecialisationDescription { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 30)]
        [Display(Name = "Specialisation Image")]
        public string SpecialisationImage { get; set; }
    }
}

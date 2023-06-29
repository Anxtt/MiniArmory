using System.ComponentModel.DataAnnotations;

using static MiniArmory.GlobalConstants.Data;

namespace MiniArmory.Data.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ClassConst.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        [StringLength(ClassConst.DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        [Required]
        [StringLength(IMAGE_MAX_LENGTH)]
        public string ClassImage { get; set; }

        [Required]
        [StringLength(ClassConst.NAME_MAX_LENGTH)]
        public string SpecialisationName { get; set; }

        [Required]
        [StringLength(ClassConst.DESCRIPTION_MAX_LENGTH)]
        public string SpecialisationDescription { get; set; }

        [Required]
        [StringLength(IMAGE_MAX_LENGTH)]
        public string SpecialisationImage { get; set; }

        [Required]
        [StringLength(IMAGE_MAX_LENGTH)]
        public string Image { get; set; }

        public ICollection<Character> Characters { get; set; } = new List<Character>();

        public ICollection<Spell> Spells { get; set; } = new List<Spell>();
    }
}

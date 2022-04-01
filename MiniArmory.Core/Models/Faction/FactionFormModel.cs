using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Faction
{
    public class FactionFormModel
    {
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        [StringLength(2000)]
        public string Image { get; set; }
    }
}

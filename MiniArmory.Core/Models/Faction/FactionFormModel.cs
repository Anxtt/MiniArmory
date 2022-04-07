using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Faction
{
    public class FactionFormModel
    {
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Name { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 50)]
        public string Description { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 30)]
        public string Image { get; set; }
    }
}

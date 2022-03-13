using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models
{
    public class FactionFormModel
    {
        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        [StringLength(2000)]
        public string Image { get; set; }
    }
}

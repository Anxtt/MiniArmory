using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models
{
    public class RoleFormModel
    {
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Role name must be between 3 and 15 characters or different than the default option.")]
        public string Name { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MiniArmory.Core.Models
{
    public class RoleFormModel
    {
        public IdentityRole<Guid> Id { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Role name must be between 3 and 15 characters.")]
        public string Name { get; set; }
    }
}

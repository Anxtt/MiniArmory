using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace MiniArmory.Data.Models
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        [StringLength(32)]
        public override string UserName { get; set; }

        public IEnumerable<Character> Characters { get; set; } = new List<Character>();
    }
}
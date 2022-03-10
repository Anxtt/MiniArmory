using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Data.Data.Models
{
    public class User
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(32)]
        public string Username { get; set; }

        [Required]
        [StringLength(60)]
        public string Email { get; set; }

        [Required]
        [StringLength(64)]
        public string Password { get; set; }

        public IEnumerable<Character> Characters { get; set; } = new List<Character>();
    }
}

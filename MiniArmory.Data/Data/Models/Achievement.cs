using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Data.Data.Models
{
    public class Achievement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public byte Points { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}

using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Data.Data.Models
{
    public class Realm
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(30)]
        public string Language { get; set; }

        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}

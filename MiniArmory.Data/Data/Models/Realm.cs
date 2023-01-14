using System.ComponentModel.DataAnnotations;

using static MiniArmory.GlobalConstants.Data;

namespace MiniArmory.Data.Data.Models
{
    public class Realm
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(RealmConst.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        [StringLength(RealmConst.LANGUAGE_MAX_LENGTH)]
        public string Language { get; set; }

        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}

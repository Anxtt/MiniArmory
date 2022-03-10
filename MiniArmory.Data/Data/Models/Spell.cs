using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniArmory.Data.Data.Models
{
    public class Spell
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [Range(5, 40)]
        public sbyte Range { get; set; }

        [Required]
        [Range(0, 600)]
        public sbyte Cooldown { get; set; }

        [Required]
        [StringLength(200)]
        public string Tooltip { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public bool IsRacial { get; set; }

        [Required]
        [ForeignKey(nameof(Class))]
        public int ClassId { get; set; }

        public Class Class { get; set; }
    }
}

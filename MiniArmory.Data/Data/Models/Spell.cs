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
        public short Cooldown { get; set; }

        [Required]
        [StringLength(200)]
        public string Tooltip { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [StringLength(15)]
        public string Type { get; set; }

        [ForeignKey(nameof(Race))]
        public int? RaceId { get; set; }

        public Race? Race { get; set; }

        [ForeignKey(nameof(Class))]
        public int? ClassId { get; set; }

        public Class? Class { get; set; }
    }
}

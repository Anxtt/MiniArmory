using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static MiniArmory.GlobalConstants.Data;

namespace MiniArmory.Data.Data.Models
{
    public class Spell
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(SpellConst.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        [Range(SpellConst.RANGE_MIN, SpellConst.RANGE_MAX)]
        public sbyte Range { get; set; }

        [Required]
        [Range(SpellConst.COOLDOWN_MIN, SpellConst.COOLDOWN_MAX)]
        public short Cooldown { get; set; }

        [Required]
        [StringLength(SpellConst.TOOLTIP_MAX_LENGTH)]
        public string Tooltip { get; set; }

        [Required]
        [StringLength(SpellConst.DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        [Required]
        [StringLength(SpellConst.TYPE_MAX_LENGTH)]
        public string Type { get; set; }

        [ForeignKey(nameof(Race))]
        public int? RaceId { get; set; }

        public Race? Race { get; set; }

        [ForeignKey(nameof(Class))]
        public int? ClassId { get; set; }

        public Class? Class { get; set; }
    }
}

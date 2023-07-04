using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static MiniArmory.GlobalConstants.Data;

namespace MiniArmory.Data.Models
{
    public class ImageData
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(ImageDataConst.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        [StringLength(ImageDataConst.CONTENTTYPE_MAX_LENGTH)]
        public string ContentType { get; set; }

        [Required]
        public byte[] OriginalContent { get; set; }

        [ForeignKey(nameof(Character))]
        public Guid? CharacterId { get; set; }

        public Character? Character { get; set; }

        //[ForeignKey(nameof(FactionImage))]
        //public int? FactionImageId { get; set; }

        //public Faction? FactionImage { get; set; }

        //[ForeignKey(nameof(ClassImage))]
        //public int? ClassImageId { get; set; }

        //public Class? ClassImage { get; set; }

        //[ForeignKey(nameof(SpecialisationImage))]
        //public int? SpecialisationImageId { get; set; }

        //public Class? SpecialisationImage { get; set; }

        //[ForeignKey(nameof(ClassHero))]
        //public int? ClassHeroId { get; set; }

        //public Class? ClassHero { get; set; }

        //[ForeignKey(nameof(MountImage))]
        //public int? MountImageId { get; set; }

        //public Mount? MountImage { get; set; }

        //[ForeignKey(nameof(RaceImage))]
        //public int? RaceImageId { get; set; }

        //public Race? RaceImage { get; set; }

        //[ForeignKey(nameof(RaceArmsImage))]
        //public int? RaceArmsImageId { get; set; }

        //public Race? RaceArmsImage { get; set; }

        //[ForeignKey(nameof(SpellImage))]
        //public int? SpellImageId { get; set; }

        //public Spell? SpellImage { get; set; }
    }
}

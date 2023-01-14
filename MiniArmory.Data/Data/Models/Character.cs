using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static MiniArmory.GlobalConstants.Data;

namespace MiniArmory.Data.Data.Models
{
    public class Character
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(CharacterConst.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        [StringLength(IMAGE_MAX_LENGTH)]
        public string Image { get; set; }

        [Required]
        [ForeignKey(nameof(Realm))]
        public int RealmId { get; set; }

        public Realm Realm { get; set; }

        [Required]
        [ForeignKey(nameof(Class))]
        public int ClassId { get; set; }

        public Class Class { get; set; }

        [Required]
        [ForeignKey(nameof(Faction))]
        public int FactionId { get; set; }

        public Faction Faction { get; set; }

        [Required]
        [ForeignKey(nameof(Race))]
        public int RaceId { get; set; }

        public Race Race { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public User User { get; set; }

        [Required]
        [Range(CharacterConst.RATING_MAX, CharacterConst.RATING_MAX)]
        public int Rating { get; set; } = CharacterConst.RATING_INITIAL;

        [Required]
        public int Win { get; set; }

        [Required]
        public int Loss { get; set; }

        public bool IsLooking { get; set; }

        [ForeignKey(nameof(Partner))]
        public Guid? PartnerId { get; set; }
        public Character? Partner { get; set; }

        public ICollection<Mount> Mounts { get; set; } = new List<Mount>();

        public ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();
    }
}

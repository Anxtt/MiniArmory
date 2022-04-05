using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniArmory.Data.Data.Models
{
    public class Character
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(16)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
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
        [Range(0, 4000)]
        public short Rating { get; set; } = 1000;

        [Required]
        public short Win { get; set; }

        [Required]
        public short Loss { get; set; }

        public bool IsLooking { get; set; }

        [ForeignKey(nameof(Partner))]
        public Guid? PartnerId { get; set; }
        public Character? Partner { get; set; }

        public ICollection<Mount> Mounts { get; set; } = new List<Mount>();

        public ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();
    }
}

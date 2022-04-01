using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models
{
    public class LeaderboardViewModel
    {
        public Guid Id { get; set; }

        public int Rating { get; set; }

        public string Name { get; set; }

        public string Realm { get; set; }

        public string Class { get; set; }

        public string Faction { get; set; }

        public short Win { get; set; }

        public short Loss { get; set; }
    }
}

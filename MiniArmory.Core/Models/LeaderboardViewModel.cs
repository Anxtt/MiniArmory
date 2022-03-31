using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models
{
    public class LeaderboardViewModel
    {
        [Range(1, int.MaxValue)]
        public int Rank { get; set; }

        public short Rating { get; set; }

        public string Name { get; set; }

        public string Realm { get; set; }

        public string Class { get; set; }

        public string Faction { get; set; }

        public short Win { get; set; }

        public short Loss { get; set; }
    }
}

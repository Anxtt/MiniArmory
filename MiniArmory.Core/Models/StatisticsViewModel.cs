using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniArmory.Core.Models
{
    public class StatisticsViewModel
    {
        public int UsersCount { get; set; }

        public int AchievementsCount { get; set; }

        public int ArenasCount { get; set; }

        public int HighestRating { get; set; }

        public JsonFormModel MostPlayedFaction { get; set; }

        public JsonFormModel MostPlayedRace { get; set; }

        public JsonFormModel MostPlayedClass { get; set; }

        public JsonFormModel MostPopulatedServer { get; set; }
    }
}

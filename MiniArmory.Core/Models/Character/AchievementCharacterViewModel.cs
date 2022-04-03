using MiniArmory.Core.Models.Achievement;

namespace MiniArmory.Core.Models.Character
{
    public class AchievementCharacterViewModel
    {
        public CharacterViewModel Character { get; set; }

        public IEnumerable<AchievementViewModel> Achievements { get; set; }
    }
}

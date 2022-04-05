namespace MiniArmory.Core.Models.Character
{
    public class LFGFormModel : CharacterViewModel
    {
        public IEnumerable<LeaderboardViewModel> CharactersInLFG { get; set; }
    }
}

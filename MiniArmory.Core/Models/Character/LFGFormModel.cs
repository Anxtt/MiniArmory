namespace MiniArmory.Core.Models.Character
{
    public class LFGFormModel : CharacterViewModel
    {
        public IEnumerable<CharacterViewModel> CharactersInLFG { get; set; }
    }
}

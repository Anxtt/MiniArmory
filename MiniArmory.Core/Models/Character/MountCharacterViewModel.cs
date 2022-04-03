using MiniArmory.Core.Models.Mount;

namespace MiniArmory.Core.Models.Character
{
    public class MountCharacterViewModel
    {
        public CharacterViewModel Character { get; set; }

        public IEnumerable<MountViewModel> Mounts { get; set; }
    }
}

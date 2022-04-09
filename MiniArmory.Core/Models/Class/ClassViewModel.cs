using MiniArmory.Core.Models.Spell;

namespace MiniArmory.Core.Models.Class
{
    public class ClassViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string ClassImage { get; set; }

        public string SpecialisationName { get; set; }

        public string SpecialisationDescription { get; set; }

        public string SpecialisationImage { get; set; }

        public List<int> SpellIds { get; set; }

        public List<SpellViewModel> Spells { get; set; }
    }
}

namespace MiniArmory.Core.Models.Character
{
    public class CharacterViewModel
    { 
        public Guid Id { get; set; }

        public int Rating { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string RealmName { get; set; }

        public string ClassName { get; set; }

        public string ClassImage { get; set; }

        public string FactionName { get; set; }

        public string FactionImage { get; set; }

        public short Win { get; set; }

        public short Loss { get; set; }
    }
}
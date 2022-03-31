namespace MiniArmory.Core.Models
{
    public class CharacterViewModel
    {
        public Guid Id { get; set; }

        public short Rating { get; set; }

        public string Name { get; set; }

        public string RealmName { get; set; }

        public string ClassName { get; set; }

        public string ClassImage { get; set; }

        public string FactionName { get; set; }

        public string FactionImage { get; set; }

        public short Win { get; set; }

        public short Loss { get; set; }
    }
}
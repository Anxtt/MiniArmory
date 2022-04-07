using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Realm
{
    public class RealmFormModel
    {
        [Required]
        [StringLength(30, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 6)]
        public string Language { get; set; }
    }
}

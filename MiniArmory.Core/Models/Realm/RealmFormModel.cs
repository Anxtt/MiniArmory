using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Realm
{
    public class RealmFormModel
    {
        [Required]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Must have a name between 5 and 30 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Must have a name between 6 and 30 characters.")]
        public string Language { get; set; }
    }
}

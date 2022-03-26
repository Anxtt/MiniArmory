﻿using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models
{
    public class RealmFormModel
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(30)]
        public string Language { get; set; }
    }
}

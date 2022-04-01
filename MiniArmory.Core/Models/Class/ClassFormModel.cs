﻿using System.ComponentModel.DataAnnotations;

namespace MiniArmory.Core.Models.Class
{
    public class ClassFormModel
    {
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(500)]
        public string Image { get; set; }

        [Required]
        [StringLength(20)]
        public string Specialisation { get; set; }

        [Required]
        [StringLength(500)]
        public string SpecialisationDescription { get; set; }

        [Required]
        [StringLength(500)]
        public string SpecialisationImage { get; set; }
    }
}
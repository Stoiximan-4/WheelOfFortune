﻿using System.ComponentModel.DataAnnotations;

namespace WheelOfFortune.Models.Domain
{
    public class WheelConfigurationSlice
    {
        public int Id { get; set; }

        [Required]
        public double Propability { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Value{get; set;}
        [Required]
        public bool Win { get; set; }
        [Required]
        public string ResultText { get; set; }
        [Required]
        public double Score { get; set; }

        public WheelConfiguration WheelConfiguration { get; set; }

        [Required]
        public ApplicationUser User { get; set; }
    }
}
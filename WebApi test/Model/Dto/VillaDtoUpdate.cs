﻿using System.ComponentModel.DataAnnotations;

namespace WebApi_test.Model.Dto
{
    public class VillaDtoUpdate
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public int Occupancy { get; set; }
        [Required]
        public int Sqft { get; set; }

        [Required]
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }

        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageLocalPath { get; set; }
        [Required]
        public string Amenity { get; set; }
    }
}

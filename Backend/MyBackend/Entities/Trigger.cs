﻿using System.ComponentModel.DataAnnotations;

namespace MyBackend.Entities
{
    public class Trigger
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "name error")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}

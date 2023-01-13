﻿using System.ComponentModel.DataAnnotations;

namespace GeoTimeConnectWebApi.Models.Request
{
    public class UserRequest
    {
        [Required]
        public string? User { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? ClientId { get; set; }

        [Required]
        public string? Schema { get; set; }

        [Required]
        public string? BDName { get; set; }
    }
}

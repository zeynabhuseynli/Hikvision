﻿namespace Hikvision.Api.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}

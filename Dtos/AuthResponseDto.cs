﻿namespace TodoApi.Dtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
    }
}

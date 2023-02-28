﻿using System.ComponentModel.DataAnnotations;

namespace BlazorChatRoom.Shared.DTOs;

public class RegisterDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required,EmailAddress]
    public string Email { get; set; }    = string.Empty;

    [Required,MinLength(6)]
    public string Password { get; set; } = string.Empty;
    
}
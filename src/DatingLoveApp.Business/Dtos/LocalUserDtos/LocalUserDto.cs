﻿namespace DatingLoveApp.Business.Dtos.LocalUserDtos;

public class LocalUserDto
{
    public Guid LocalUserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string UserName { get; set; } = null!;

    public int Age { get; set; }

    public string Gender { get; set; } = null!;

    public string? KnownAs { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public DateTime LastActive { get; set; }

    public string? Address { get; set; }

    public string? Ward { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }
}

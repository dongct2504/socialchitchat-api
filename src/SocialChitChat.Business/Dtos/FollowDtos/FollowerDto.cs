﻿namespace SocialChitChat.Business.Dtos.FollowDtos;

public class FollowerDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string Nickname { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? City { get; set; }
}

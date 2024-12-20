﻿namespace SocialChitChat.Business.Dtos.AppUsers;

public class AppUserWithRolesDto
{
    public string Id { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public List<string> Roles { get; set; } = new List<string>();
}

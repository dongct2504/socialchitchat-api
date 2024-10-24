﻿using SocialChitChat.Business.Common.Constants;

namespace SocialChitChat.Business.Dtos.MessageDtos;

public class MessageParams
{
    public Guid Id { get; set; }

    public string Contain { get; set; } = MessageConstants.Inbox;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 6;

    public override string? ToString()
    {
        return $"{Id}-{Contain}-{PageNumber}-{PageSize}";
    }
}

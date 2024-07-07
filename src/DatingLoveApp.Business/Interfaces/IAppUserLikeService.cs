﻿using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.AppUserLikes;
using FluentResults;

namespace DatingLoveApp.Business.Interfaces;

public interface IAppUserLikeService
{
    Task<Result<PagedList<LikeDto>>> GetUserLikesAsync(AppUserLikeParams likeParams);

    Task<Result> UpdateLikeAsync(string sourceUserId, string likedUserId);
}

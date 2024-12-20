﻿using FluentResults;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SocialChitChat.Business.Common.Constants;
using SocialChitChat.Business.Common.Errors;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.AdminDtos;
using SocialChitChat.Business.Dtos.AppUsers;
using SocialChitChat.Business.Dtos.PictureDtos;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.DataAccess.Common;
using SocialChitChat.DataAccess.Identity;

namespace SocialChitChat.Business.Services;

public class AdminService : IAdminService
{
    private readonly UserManager<AppUser> _userManager;

    public AdminService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<PagedList<AppUserWithRolesDto>> GetUsersWithRolesAsync(
        UsersWithRolesParams usersWithRolesParams)
    {
        IQueryable<AppUser> query = _userManager.Users.AsQueryable();

        switch (usersWithRolesParams.SortBy)
        {
            case UserSortConstants.LastActive:
                query = query.OrderByDescending(u => u.LastActive);
                break;

            case UserSortConstants.Created:
                query = query.OrderByDescending(u => u.CreatedAt);
                break;

            case UserSortConstants.Nickname:
                query = query.OrderBy(u => u.Nickname);
                break;
        }

        int totalRecords = await query.CountAsync();

        List<AppUserWithRolesDto> appUserWithRolesDtos = await query
            .AsNoTracking()
            .ProjectToType<AppUserWithRolesDto>() // no need to Include() and ThenInlude()
            .ToListAsync();

        PagedList<AppUserWithRolesDto> pagedList = new PagedList<AppUserWithRolesDto>
        {
            PageNumber = usersWithRolesParams.PageNumber,
            PageSize = usersWithRolesParams.PageSize,
            TotalRecords = totalRecords,
            Items = appUserWithRolesDtos
        };

        return pagedList;
    }

    public async Task<Result<string[]>> EditRolesAsync(string id, string roles)
    {
        string[] selectedRoles = roles
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(r => r.Trim().ToLower())
            .ToArray();
        string[] allowedRoles = { RoleConstants.User, RoleConstants.Employee, RoleConstants.Admin };

        foreach (string role in selectedRoles)
        {
            if (!allowedRoles.Contains(role))
            {
                string message = $"Invalid role: {role}";
                Log.Warning($"{nameof(EditRolesAsync)} - {message} - {typeof(AdminService)}");
                return Result.Fail(new BadRequestError(message));
            }
        }

        AppUser? user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(EditRolesAsync)} - {message} - {typeof(AdminService)}");
            return Result.Fail(new NotFoundError(message));
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
        if (!result.Succeeded)
        {
            string message = "Failed to add to roles.";
            Log.Warning($"{nameof(EditRolesAsync)} - {message} - {typeof(AdminService)}");
            return Result.Fail(new BadRequestError(message));
        }

        result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
        if (!result.Succeeded)
        {
            string message = "Failed to remove from roles.";
            Log.Warning($"{nameof(EditRolesAsync)} - {message} - {typeof(AdminService)}");
            return Result.Fail(new BadRequestError(message));
        }

        return (await _userManager.GetRolesAsync(user)).ToArray();
    }

    public Task<PictureDto> PictureToModerateAsync()
    {
        throw new NotImplementedException();
    }
}

using Constants;
using Dtos;
using ELF.Common.Models;
using Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELF.Web.Endpoints;

public static class Users
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(nameof(Users))
            .WithName(nameof(Users))
            .WithDisplayName("用户管理")
            .WithTags(nameof(Users), "用户管理")
            .RequireAuthorization(PermissionConstants.PolicyNames)
        ;
        group.MapGet("{id}", GetAysnc)
            .WithName($"User{nameof(GetAysnc)}")
            .WithTags(PermissionConstants.Users_Detail)
            .WithDisplayName("获取用户详情");
        group.MapGet("", GetListAsync)
            .WithName($"User{nameof(GetListAsync)}")
            .WithTags(PermissionConstants.Users_List)
            .WithDisplayName("获取用户列表");
        group.MapPost("", CreateAsync)
            .WithName($"User{nameof(CreateAsync)}")
            .WithTags(PermissionConstants.Users_Create)
            .WithDisplayName("创建用户");
        group.MapPut("{id}", UpdateAsync)
            .WithName($"User{nameof(UpdateAsync)}")
            .WithTags(PermissionConstants.Users_Update)
            .WithDisplayName("更新用户");
        group.MapDelete("{id}", DeleteAsync)
            .WithName($"User{nameof(DeleteAsync)}")
            .WithTags(PermissionConstants.Users_Delete)
            .WithDisplayName("删除用户");
        group.MapPost("{id}/ResetPassword", ResetPasswordAsync)
            .WithName($"User{nameof(ResetPasswordAsync)}")
            .WithTags(PermissionConstants.Users_ResetPassword)
            .WithDisplayName("重置密码");
        group.MapGet("{id}/Roles", GetRolesAsync)
            .WithName($"User{nameof(GetRolesAsync)}")
            .WithTags(PermissionConstants.Users_GetRoles)
            .WithDisplayName("获取用户角色");
        group.MapPut("{id}/Roles", SetRolesAsync)
            .WithName($"User{nameof(SetRolesAsync)}")
            .WithTags(PermissionConstants.Users_SetRoles)
            .WithDisplayName("设置用户角色");
        group.MapGet("CurrentUser", GetCurrentUserAsync)
            .WithName($"User{nameof(GetCurrentUserAsync)}")
            .WithTags(PermissionConstants.Users_GetCurrentUser)
            .WithDisplayName("获取当前用户信息");
        group.MapPut("UpdatePassword", UpdatePasswordAsync)
            .WithName($"User{nameof(UpdatePasswordAsync)}")
            .WithTags(PermissionConstants.Users_UpdatePassword)
            .WithDisplayName("修改密码");
    }

    public static async Task<Ok<UserDto>> GetAysnc(ISender sender, long id)
    {
        var result = await sender.Send(new UserQuery(id));

        return TypedResults.Ok(result);
    }

    public static async Task<Ok<AmisPagedOutput<UserGetListOutput>>> GetListAsync(ISender sender, [AsParameters] UserGetListQuery query)
    {
        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public static async Task<Created<UserDto>> CreateAsync(ISender sender, UserCreateCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Users)}/{id}", id);
    }

    public static async Task<Results<NoContent, BadRequest>> UpdateAsync(ISender sender, long id, UserUpdateCommand command)
    {
        if (id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public static async Task<NoContent> DeleteAsync(ISender sender, long id)
    {
        await sender.Send(new UserDeleteCommand(id));

        return TypedResults.NoContent();
    }

    public static async Task<NoContent> ResetPasswordAsync(ISender sender, long id)
    {
        await sender.Send(new UserResetPasswordCommand(id));

        return TypedResults.NoContent();
    }

    public static async Task<Ok<string>> GetRolesAsync(ISender sender, long id)
    {
        var permissions = await sender.Send(new UserGetRolesQuery(id));

        return TypedResults.Ok(permissions ?? "");
    }

    public static async Task<NoContent> SetRolesAsync(ISender sender, long id, UserRolesCommand command)
    {
        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public static async Task<Results<Ok<UserDto>, ForbidHttpResult>> GetCurrentUserAsync(ISender sender, IUser user)
    {
        if (user.Id == null) return TypedResults.Forbid();
        long userId = long.Parse(user.Id);
        var result = await sender.Send(new UserQuery(userId));
        return TypedResults.Ok(result);
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    public static async Task<Results<NoContent, BadRequest<string>, ForbidHttpResult>> UpdatePasswordAsync(ISender sender, IUser user, UpdatePasswordRequest request)
    {
        if (request.NewPassword != request.ConfirmNewPassword) return TypedResults.BadRequest("两次输入的密码不一致");
        if (user.Id == null) return TypedResults.Forbid();
        long userId = long.Parse(user.Id);
        await sender.Send(new UpdatePasswordCommand(userId, request.OldPassword, request.NewPassword));
        return TypedResults.NoContent();
    }
}

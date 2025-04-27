using Constants;
using Dtos;
using ELF.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELF.Web.Endpoints;

public static class Roles
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(nameof(Roles))
            .WithName(nameof(Roles))
            .WithDisplayName("角色管理")
            .WithTags(nameof(Roles), "角色管理")
        .RequireAuthorization(PermissionConstants.PolicyNames);
        ;
        group.MapGet("{id}", GetAysnc)
            .WithName($"Role{nameof(GetAysnc)}")
            .WithTags(PermissionConstants.Roles_Detail)
            .WithDisplayName("获取角色详情");
        group.MapGet("", GetListAsync)
            .WithName($"Role{nameof(GetListAsync)}")
            .WithTags(PermissionConstants.Roles_List)
            .WithDisplayName("获取角色列表");
        group.MapPost("", CreateAsync)
            .WithName($"Role{nameof(CreateAsync)}")
            .WithTags(PermissionConstants.Roles_Create)
            .WithDisplayName("创建角色");
        group.MapPut("{id}", UpdateAsync)
            .WithName($"Role{nameof(UpdateAsync)}")
            .WithTags(PermissionConstants.Roles_Update)
            .WithDisplayName("更新角色");
        group.MapDelete("{id}", DeleteAsync)
            .WithName($"Role{nameof(DeleteAsync)}")
            .WithTags(PermissionConstants.Roles_Delete)
            .WithDisplayName("删除角色");
        group.MapGet("{id}/Permissions", GetPermissionsAsync)
            .WithName($"Role{nameof(GetPermissionsAsync)}")
            .WithTags(PermissionConstants.Roles_GetPermissions)
            .WithDisplayName("获取角色权限");
        group.MapPut("{id}/Permissions", PermissionsAsync)
            .WithName($"Role{nameof(PermissionsAsync)}")
            .WithTags(PermissionConstants.Roles_SetPermissions)
            .WithDisplayName("分配权限");
    }

    public static async Task<Ok<RoleDto>> GetAysnc(ISender sender, long id)
    {
        var result = await sender.Send(new RoleQuery(id));

        return TypedResults.Ok(result);
    }

    public static async Task<Ok<AmisPagedOutput<RoleGetListOutput>>> GetListAsync(ISender sender, [AsParameters] RoleGetListQuery query)
    {
        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public static async Task<Created<RoleDto>> CreateAsync(ISender sender, RoleCreateCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Roles)}/{id}", id);
    }

    public static async Task<Results<NoContent, BadRequest>> UpdateAsync(ISender sender, long id, RoleUpdateCommand command)
    {
        if (id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public static async Task<NoContent> DeleteAsync(ISender sender, long id)
    {
        await sender.Send(new RoleDeleteCommand(id));

        return TypedResults.NoContent();
    }

    public static async Task<Ok<string>> GetPermissionsAsync(ISender sender, long id)
    {
        var permissions = await sender.Send(new RoleGetPermissionsQuery(id));

        return TypedResults.Ok(permissions ?? "");
    }

    public static async Task<NoContent> PermissionsAsync(ISender sender, long id, RolePermissionsCommand command)
    {
        await sender.Send(command);

        return TypedResults.NoContent();
    }
}

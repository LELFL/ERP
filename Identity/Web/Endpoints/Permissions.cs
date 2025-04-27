using Constants;
using Dtos;
using ELF.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELF.Web.Endpoints;

public static class Permissions
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(nameof(Permissions))
            .WithName("Permissions")
            .WithDisplayName("权限管理")
            .WithTags(nameof(Permissions), "权限管理")
            .RequireAuthorization(PermissionConstants.PolicyNames)
        ;
        group.MapGet("{id}", GetAysnc)
            .WithName($"Permission{nameof(GetAysnc)}")
            .WithTags(PermissionConstants.Permissions_Detail)
            .WithDisplayName("获取权限详情");
        group.MapGet("", GetListAsync)
            .WithName($"Permission{nameof(GetListAsync)}")
            .WithTags(PermissionConstants.Permissions_List)
            .WithDisplayName("获取权限列表");
        group.MapPost("", CreateAsync)
            .WithName($"Permission{nameof(CreateAsync)}")
            .WithTags(PermissionConstants.Permissions_Create)
            .WithDisplayName("创建权限");
        group.MapPut("{id}", UpdateAsync)
            .WithName($"Permission{nameof(UpdateAsync)}")
            .WithTags(PermissionConstants.Permissions_Update)
            .WithDisplayName("更新权限");
        group.MapDelete("{id}", DeleteAsync)
            .WithName($"Permission{nameof(DeleteAsync)}")
            .WithTags(PermissionConstants.Permissions_Delete)
            .WithDisplayName("删除权限");
    }

    public static async Task<Ok<PermissionDto>> GetAysnc(ISender sender, long id)
    {
        var result = await sender.Send(new PermissionQuery(id));

        return TypedResults.Ok(result);
    }

    public static async Task<Ok<AmisPagedOutput<PermissionGetListOutput>>> GetListAsync(ISender sender, [AsParameters] PermissionGetListQuery query)
    {
        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public static async Task<Created<PermissionDto>> CreateAsync(ISender sender, PermissionCreateCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Permissions)}/{id}", id);
    }

    public static async Task<Results<NoContent, BadRequest>> UpdateAsync(ISender sender, long id, PermissionUpdateCommand command)
    {
        if (id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public static async Task<NoContent> DeleteAsync(ISender sender, long id)
    {
        await sender.Send(new PermissionDeleteCommand(id));

        return TypedResults.NoContent();
    }
}

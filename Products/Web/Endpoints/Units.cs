using Constants;
using Dtos;
using ELF.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELF.Web.Endpoints;

public static class Units
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(nameof(Units))
            .WithName("Units")
            .WithDisplayName("计量单位管理")
            .WithTags(nameof(Units), "计量单位管理")
            .RequireAuthorization(PermissionConstants.PolicyNames)
        ;
        group.MapGet("{id}", GetAysnc)
            .WithName($"Unit{nameof(GetAysnc)}")
            .WithPermission(PermissionConstants.Units_Detail)
            .WithDisplayName("获取计量单位详情");
        group.MapGet("", GetListAsync)
            .WithName($"Unit{nameof(GetListAsync)}")
            .WithPermission(PermissionConstants.Units_List)
            .WithDisplayName("获取计量单位列表");
        group.MapPost("", CreateAsync)
            .WithName($"Unit{nameof(CreateAsync)}")
            .WithPermission(PermissionConstants.Units_Create)
            .WithDisplayName("创建计量单位");
        group.MapPut("{id}", UpdateAsync)
            .WithName($"Unit{nameof(UpdateAsync)}")
            .WithPermission(PermissionConstants.Units_Update)
            .WithDisplayName("更新计量单位");
        group.MapDelete("{id}", DeleteAsync)
            .WithName($"Unit{nameof(DeleteAsync)}")
            .WithPermission(PermissionConstants.Units_Delete)
            .WithDisplayName("删除计量单位");
    }

    public static async Task<Ok<UnitDto>> GetAysnc(ISender sender, long id)
    {
        var result = await sender.Send(new UnitQuery(id));

        return TypedResults.Ok(result);
    }

    public static async Task<Ok<AmisPagedOutput<UnitGetListOutput>>> GetListAsync(ISender sender, [AsParameters] UnitGetListQuery query)
    {
        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public static async Task<Created<UnitDto>> CreateAsync(ISender sender, UnitCreateCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Units)}/{id}", id);
    }

    public static async Task<Results<NoContent, BadRequest>> UpdateAsync(ISender sender, long id, UnitUpdateCommand command)
    {
        if (id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public static async Task<NoContent> DeleteAsync(ISender sender, long id)
    {
        await sender.Send(new UnitDeleteCommand(id));

        return TypedResults.NoContent();
    }
}

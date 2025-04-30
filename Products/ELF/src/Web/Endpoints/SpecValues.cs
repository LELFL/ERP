using Constants;
using Dtos;
using ELF.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELF.Web.Endpoints;

public static class SpecValues
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(nameof(SpecValues))
            .WithName("SpecValues")
            .WithDisplayName("商品规格值管理")
            .WithTags(nameof(SpecValues), "商品规格值管理")
            .RequireAuthorization(PermissionConstants.PolicyNames)
        ;
        group.MapGet("{id}", GetAysnc)
            .WithName($"SpecValue{nameof(GetAysnc)}")
            .WithTags(PermissionConstants.SpecValues_Detail)
            .WithDisplayName("获取商品规格值详情");
        group.MapGet("", GetListAsync)
            .WithName($"SpecValue{nameof(GetListAsync)}")
            .WithTags(PermissionConstants.SpecValues_List)
            .WithDisplayName("获取商品规格值列表");
        group.MapPost("", CreateAsync)
            .WithName($"SpecValue{nameof(CreateAsync)}")
            .WithTags(PermissionConstants.SpecValues_Create)
            .WithDisplayName("创建商品规格值");
        group.MapPut("{id}", UpdateAsync)
            .WithName($"SpecValue{nameof(UpdateAsync)}")
            .WithTags(PermissionConstants.SpecValues_Update)
            .WithDisplayName("更新商品规格值");
        group.MapDelete("{id}", DeleteAsync)
            .WithName($"SpecValue{nameof(DeleteAsync)}")
            .WithTags(PermissionConstants.SpecValues_Delete)
            .WithDisplayName("删除商品规格值");
    }

    public static async Task<Ok<SpecValueDto>> GetAysnc(ISender sender, long id)
    {
        var result = await sender.Send(new SpecValueQuery(id));

        return TypedResults.Ok(result);
    }

    public static async Task<Ok<AmisPagedOutput<SpecValueGetListOutput>>> GetListAsync(ISender sender, [AsParameters] SpecValueGetListQuery query)
    {
        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public static async Task<Created<SpecValueDto>> CreateAsync(ISender sender, SpecValueCreateCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(SpecValues)}/{id}", id);
    }

    public static async Task<Results<NoContent, BadRequest>> UpdateAsync(ISender sender, long id, SpecValueUpdateCommand command)
    {
        if (id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public static async Task<NoContent> DeleteAsync(ISender sender, long id)
    {
        await sender.Send(new SpecValueDeleteCommand(id));

        return TypedResults.NoContent();
    }
}

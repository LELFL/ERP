using Constants;
using Dtos;
using ELF.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELF.Web.Endpoints;

public static class Specs
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(nameof(Specs))
            .WithName("Specs")
            .WithDisplayName("商品规格名称管理")
            .WithTags(nameof(Specs), "商品规格名称管理")
            .RequireAuthorization(PermissionConstants.PolicyNames)
        ;
        group.MapGet("{id}", GetAysnc)
            .WithName($"Spec{nameof(GetAysnc)}")
            .WithTags(PermissionConstants.Specs_Detail)
            .WithDisplayName("获取商品规格名称详情");
        group.MapGet("", GetListAsync)
            .WithName($"Spec{nameof(GetListAsync)}")
            .WithTags(PermissionConstants.Specs_List)
            .WithDisplayName("获取商品规格名称列表");
        group.MapPost("", CreateAsync)
            .WithName($"Spec{nameof(CreateAsync)}")
            .WithTags(PermissionConstants.Specs_Create)
            .WithDisplayName("创建商品规格名称");
        group.MapPut("{id}", UpdateAsync)
            .WithName($"Spec{nameof(UpdateAsync)}")
            .WithTags(PermissionConstants.Specs_Update)
            .WithDisplayName("更新商品规格名称");
        group.MapDelete("{id}", DeleteAsync)
            .WithName($"Spec{nameof(DeleteAsync)}")
            .WithTags(PermissionConstants.Specs_Delete)
            .WithDisplayName("删除商品规格名称");
    }

    public static async Task<Ok<SpecDto>> GetAysnc(ISender sender, long id)
    {
        var result = await sender.Send(new SpecQuery(id));

        return TypedResults.Ok(result);
    }

    public static async Task<Ok<AmisPagedOutput<SpecGetListOutput>>> GetListAsync(ISender sender, [AsParameters] SpecGetListQuery query)
    {
        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public static async Task<Created<SpecDto>> CreateAsync(ISender sender, SpecCreateCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Specs)}/{id}", id);
    }

    public static async Task<Results<NoContent, BadRequest>> UpdateAsync(ISender sender, long id, SpecUpdateCommand command)
    {
        if (id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public static async Task<NoContent> DeleteAsync(ISender sender, long id)
    {
        await sender.Send(new SpecDeleteCommand(id));

        return TypedResults.NoContent();
    }
}

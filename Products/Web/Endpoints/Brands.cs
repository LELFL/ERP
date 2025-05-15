using Constants;
using Dtos;
using ELF.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELF.Web.Endpoints;

public static class Brands
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(nameof(Brands))
            .WithName("Brands")
            .WithDisplayName("商品品牌管理")
            .WithTags(nameof(Brands), "商品品牌管理")
            .RequireAuthorization(PermissionConstants.PolicyNames)
        ;
        group.MapGet("{id}", GetAysnc)
            .WithName($"Brand{nameof(GetAysnc)}")
            .WithPermission(PermissionConstants.Brands_Detail)
            .WithDisplayName("获取商品品牌详情");
        group.MapGet("", GetListAsync)
            .WithName($"Brand{nameof(GetListAsync)}")
            .WithPermission(PermissionConstants.Brands_List)
            .WithDisplayName("获取商品品牌列表");
        group.MapPost("", CreateAsync)
            .WithName($"Brand{nameof(CreateAsync)}")
            .WithPermission(PermissionConstants.Brands_Create)
            .WithDisplayName("创建商品品牌");
        group.MapPut("{id}", UpdateAsync)
            .WithName($"Brand{nameof(UpdateAsync)}")
            .WithPermission(PermissionConstants.Brands_Update)
            .WithDisplayName("更新商品品牌");
        group.MapDelete("{id}", DeleteAsync)
            .WithName($"Brand{nameof(DeleteAsync)}")
            .WithPermission(PermissionConstants.Brands_Delete)
            .WithDisplayName("删除商品品牌");
    }

    public static async Task<Ok<BrandDto>> GetAysnc(ISender sender, long id)
    {
        var result = await sender.Send(new BrandQuery(id));

        return TypedResults.Ok(result);
    }

    public static async Task<Ok<AmisPagedOutput<BrandGetListOutput>>> GetListAsync(ISender sender, [AsParameters] BrandGetListQuery query)
    {
        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public static async Task<Created<BrandDto>> CreateAsync(ISender sender, BrandCreateCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Brands)}/{id}", id);
    }

    public static async Task<Results<NoContent, BadRequest>> UpdateAsync(ISender sender, long id, BrandUpdateCommand command)
    {
        if (id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public static async Task<NoContent> DeleteAsync(ISender sender, long id)
    {
        await sender.Send(new BrandDeleteCommand(id));

        return TypedResults.NoContent();
    }
}

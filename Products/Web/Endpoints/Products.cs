using Constants;
using Dtos;
using ELF.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELF.Web.Endpoints;

public static class Products
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(nameof(Products))
            .WithName("Products")
            .WithDisplayName("商品管理")
            .WithTags(nameof(Products), "商品管理")
            .RequireAuthorization(PermissionConstants.PolicyNames)
        ;
        group.MapGet("{id}", GetAysnc)
            .WithName($"Product{nameof(GetAysnc)}")
            .WithPermission(PermissionConstants.Products_Detail)
            .WithDisplayName("获取商品详情");
        group.MapGet("", GetListAsync)
            .WithName($"Product{nameof(GetListAsync)}")
            .WithPermission(PermissionConstants.Products_List)
            .WithDisplayName("获取商品列表");
        group.MapPost("", CreateAsync)
            .WithName($"Product{nameof(CreateAsync)}")
            .WithPermission(PermissionConstants.Products_Create)
            .WithDisplayName("创建商品");
        group.MapPut("{id}", UpdateAsync)
            .WithName($"Product{nameof(UpdateAsync)}")
            .WithPermission(PermissionConstants.Products_Update)
            .WithDisplayName("更新商品");
        group.MapDelete("{id}", DeleteAsync)
            .WithName($"Product{nameof(DeleteAsync)}")
            .WithPermission(PermissionConstants.Products_Delete)
            .WithDisplayName("删除商品");
    }

    public static async Task<Ok<ProductDto>> GetAysnc(ISender sender, long id)
    {
        var result = await sender.Send(new ProductQuery(id));

        return TypedResults.Ok(result);
    }

    public static async Task<Ok<AmisPagedOutput<ProductGetListOutput>>> GetListAsync(ISender sender, [AsParameters] ProductGetListQuery query)
    {
        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public static async Task<Created<ProductDto>> CreateAsync(ISender sender, ProductCreateCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Products)}/{id}", id);
    }

    public static async Task<Results<NoContent, BadRequest>> UpdateAsync(ISender sender, long id, ProductUpdateCommand command)
    {
        if (id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public static async Task<NoContent> DeleteAsync(ISender sender, long id)
    {
        await sender.Send(new ProductDeleteCommand(id));

        return TypedResults.NoContent();
    }
}

using Constants;
using Dtos;
using ELF.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELF.Web.Endpoints;

public static class ProductSKUs
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(nameof(ProductSKUs))
            .WithName("ProductSKUs")
            .WithDisplayName("商品规格管理")
            .WithTags(nameof(ProductSKUs), "商品规格管理")
            .RequireAuthorization(PermissionConstants.PolicyNames)
        ;
        group.MapGet("{id}", GetAysnc)
            .WithName($"ProductSKU{nameof(GetAysnc)}")
            .WithPermission(PermissionConstants.ProductSKUs_Detail)
            .WithDisplayName("获取商品规格详情");
        group.MapGet("", GetListAsync)
            .WithName($"ProductSKU{nameof(GetListAsync)}")
            .WithPermission(PermissionConstants.ProductSKUs_List)
            .WithDisplayName("获取商品规格列表");
        group.MapPost("", CreateAsync)
            .WithName($"ProductSKU{nameof(CreateAsync)}")
            .WithPermission(PermissionConstants.ProductSKUs_Create)
            .WithDisplayName("创建商品规格");
        group.MapPut("{id}", UpdateAsync)
            .WithName($"ProductSKU{nameof(UpdateAsync)}")
            .WithPermission(PermissionConstants.ProductSKUs_Update)
            .WithDisplayName("更新商品规格");
        group.MapDelete("{id}", DeleteAsync)
            .WithName($"ProductSKU{nameof(DeleteAsync)}")
            .WithPermission(PermissionConstants.ProductSKUs_Delete)
            .WithDisplayName("删除商品规格");
    }

    public static async Task<Ok<ProductSKUDto>> GetAysnc(ISender sender, long id)
    {
        var result = await sender.Send(new ProductSKUQuery(id));

        return TypedResults.Ok(result);
    }

    public static async Task<Ok<AmisPagedOutput<ProductSKUGetListOutput>>> GetListAsync(ISender sender, [AsParameters] ProductSKUGetListQuery query)
    {
        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public static async Task<Created<ProductSKUDto>> CreateAsync(ISender sender, ProductSKUCreateCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(ProductSKUs)}/{id}", id);
    }

    public static async Task<Results<NoContent, BadRequest>> UpdateAsync(ISender sender, long id, ProductSKUUpdateCommand command)
    {
        if (id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public static async Task<NoContent> DeleteAsync(ISender sender, long id)
    {
        await sender.Send(new ProductSKUDeleteCommand(id));

        return TypedResults.NoContent();
    }
}

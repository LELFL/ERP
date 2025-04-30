using Constants;
using Dtos;
using ELF.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELF.Web.Endpoints;

public static class Categorys
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(nameof(Categorys))
            .WithName("Categorys")
            .WithDisplayName("商品分类管理")
            .WithTags(nameof(Categorys), "商品分类管理")
            .RequireAuthorization(PermissionConstants.PolicyNames)
        ;
        group.MapGet("{id}", GetAysnc)
            .WithName($"Category{nameof(GetAysnc)}")
            .WithTags(PermissionConstants.Categorys_Detail)
            .WithDisplayName("获取商品分类详情");
        group.MapGet("", GetListAsync)
            .WithName($"Category{nameof(GetListAsync)}")
            .WithTags(PermissionConstants.Categorys_List)
            .WithDisplayName("获取商品分类列表");
        group.MapPost("", CreateAsync)
            .WithName($"Category{nameof(CreateAsync)}")
            .WithTags(PermissionConstants.Categorys_Create)
            .WithDisplayName("创建商品分类");
        group.MapPut("{id}", UpdateAsync)
            .WithName($"Category{nameof(UpdateAsync)}")
            .WithTags(PermissionConstants.Categorys_Update)
            .WithDisplayName("更新商品分类");
        group.MapDelete("{id}", DeleteAsync)
            .WithName($"Category{nameof(DeleteAsync)}")
            .WithTags(PermissionConstants.Categorys_Delete)
            .WithDisplayName("删除商品分类");
    }

    public static async Task<Ok<CategoryDto>> GetAysnc(ISender sender, long id)
    {
        var result = await sender.Send(new CategoryQuery(id));

        return TypedResults.Ok(result);
    }

    public static async Task<Ok<AmisPagedOutput<CategoryGetListOutput>>> GetListAsync(ISender sender, [AsParameters] CategoryGetListQuery query)
    {
        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public static async Task<Created<CategoryDto>> CreateAsync(ISender sender, CategoryCreateCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Categorys)}/{id}", id);
    }

    public static async Task<Results<NoContent, BadRequest>> UpdateAsync(ISender sender, long id, CategoryUpdateCommand command)
    {
        if (id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public static async Task<NoContent> DeleteAsync(ISender sender, long id)
    {
        await sender.Send(new CategoryDeleteCommand(id));

        return TypedResults.NoContent();
    }
}

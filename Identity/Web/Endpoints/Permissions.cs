using Constants;
using Dtos;
using ELF.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using OfficeOpenXml;

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
            .WithPermission(PermissionConstants.Permissions_Detail)
            .WithDisplayName("获取权限详情");
        group.MapGet("", GetListAsync)
            .WithName($"Permission{nameof(GetListAsync)}")
            .WithPermission(PermissionConstants.Permissions_List)
            .WithDisplayName("获取权限列表");
        group.MapPost("", CreateAsync)
            .WithName($"Permission{nameof(CreateAsync)}")
            .WithPermission(PermissionConstants.Permissions_Create)
            .WithDisplayName("创建权限");
        group.MapPut("{id}", UpdateAsync)
            .WithName($"Permission{nameof(UpdateAsync)}")
            .WithPermission(PermissionConstants.Permissions_Update)
            .WithDisplayName("更新权限");
        group.MapDelete("{id}", DeleteAsync)
            .WithName($"Permission{nameof(DeleteAsync)}")
            .WithPermission(PermissionConstants.Permissions_Delete)
            .WithDisplayName("删除权限");
        group.MapDelete("/Upload", UploadAsync)
            .WithName($"Permission{nameof(UploadAsync)}")
            .WithPermission(PermissionConstants.Permissions_Upload)
            .WithDisplayName("导入权限")
            .Accepts<IFormFile>("multipart/form-data");
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

    public static async Task<Results<NoContent, BadRequest<string>>> UploadAsync(
        ISender sender, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return TypedResults.BadRequest("未上传文件");

        var permissions = new List<PermissionImportCommand>();

        try
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++) // 假设第一行为表头
            {
                var name = worksheet.Cells[row, 1].Text?.Trim();
                var description = worksheet.Cells[row, 2].Text?.Trim();
                var sortText = worksheet.Cells[row, 3].Text?.Trim();
                var parentName = worksheet.Cells[row, 4].Text?.Trim();

                if (string.IsNullOrWhiteSpace(name))
                    continue; // 跳过无效行

                var command = new PermissionImportCommand
                {
                    Name = name,
                    Description = description ?? "",
                    Sort = int.TryParse(sortText, out var sort) ? sort : 0,
                    ParentName = string.IsNullOrWhiteSpace(parentName) ? null : parentName
                };
                permissions.Add(command);
            }

            foreach (var cmd in permissions)
            {
                await sender.Send(cmd);
            }
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest($"导入失败: {ex.Message}");
        }

        return TypedResults.NoContent();
    }
}

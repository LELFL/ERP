#nullable enable
// 自动生成代码，请不要修改
using ELF.Common.Models;
namespace Dtos;

/// <summary>
/// 权限
/// </summary>
public partial class PermissionGetListQuery : AmisGetListInputBase, IRequest<AmisPagedOutput<PermissionGetListOutput>>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int? Sort { get; set; }

    /// <summary>
    /// 父级权限
    /// </summary>
    public long? ParentId { get; set; }

}
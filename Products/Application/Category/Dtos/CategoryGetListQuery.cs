#nullable enable
// 自动生成代码，请不要修改
using ELF.Common.Models;
namespace Dtos;

/// <summary>
/// 商品分类
/// </summary>
public partial class CategoryGetListQuery : AmisGetListInputBase, IRequest<AmisPagedOutput<CategoryGetListOutput>>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 上级分类ID
    /// </summary>
    public long? ImageId { get; set; }

    /// <summary>
    /// 排序方式
    /// </summary>
    public int? Sort { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool? Enable { get; set; }

    /// <summary>
    /// 上级分类ID
    /// </summary>
    public long? ParentId { get; set; }

}
#nullable enable
// 自动生成代码，请不要修改
using ELF.Common.Models;
namespace Dtos;

/// <summary>
/// 商品
/// </summary>
public partial class ProductGetListQuery : AmisGetListInputBase, IRequest<AmisPagedOutput<ProductGetListOutput>>
{
    /// <summary>
    /// 编号
    /// </summary>
    public string? No { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 分类ID
    /// </summary>
    public long? CategoryId { get; set; }

    /// <summary>
    /// 品牌ID
    /// </summary>
    public long? BrandId { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool? Enable { get; set; }

    /// <summary>
    /// 款式
    /// </summary>
    public string? Style { get; set; }

    /// <summary>
    /// 型号
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// 规格
    /// </summary>
    public string? Specification { get; set; }

    /// <summary>
    /// 单位ID
    /// </summary>
    public long? UnitId { get; set; }

}
#nullable enable
// 自动生成代码，请不要修改
using ELF.Common.Models;
namespace Dtos;

/// <summary>
/// 商品规格
/// </summary>
public partial class ProductSKUGetListQuery : AmisGetListInputBase, IRequest<AmisPagedOutput<ProductSKUGetListOutput>>
{
    /// <summary>
    /// 编码
    /// </summary>
    public string? No { get; set; }

    /// <summary>
    /// 商品ID
    /// </summary>
    public long? ProductId { get; set; }

    /// <summary>
    /// 图片ID
    /// </summary>
    public long? ImageId { get; set; }

    /// <summary>
    /// 重量
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// 商品价格
    /// </summary>
    public decimal? Price { get; set; }

}
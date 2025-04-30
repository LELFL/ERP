#nullable enable
// 自动生成代码，请不要修改
using ELF.Common.Models;
namespace Dtos;

/// <summary>
/// 商品品牌
/// </summary>
public partial class BrandGetListQuery : AmisGetListInputBase, IRequest<AmisPagedOutput<BrandGetListOutput>>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }
    
}
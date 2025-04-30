#nullable enable
// 自动生成代码，请不要修改
namespace Dtos;

/// <summary>
/// 商品规格值
/// </summary>
public partial class SpecValueCreateCommand : IRequest<SpecValueDto>
{
    /// <summary>
    /// 商品规格ID
    /// </summary>
    public long ProductSpecId { get; set; } 
    
    /// <summary>
    /// 规格值
    /// </summary>
    public string Value { get; set; }  = default!;
    
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }  = 0;
    
}
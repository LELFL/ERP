#nullable enable
// 自动生成代码，请不要修改
namespace Dtos;

/// <summary>
/// 商品规格
/// </summary>
public partial class ProductSKUGetListOutput : BaseDto<long>
{
    /// <summary>
    /// 编码
    /// </summary>
    public string No { get; set; }  = default!;
    
    /// <summary>
    /// 商品ID
    /// </summary>
    public long ProductId { get; set; } 
    
    /// <summary>
    /// 图片ID
    /// </summary>
    public long? ImageId { get; set; } 
    
    /// <summary>
    /// 重量
    /// </summary>
    public decimal Weight { get; set; }  = 0m;
    
    /// <summary>
    /// 商品价格
    /// </summary>
    public decimal Price { get; set; }  = 0m;
    
}
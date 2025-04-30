#nullable enable
// 自动生成代码，请不要修改
namespace Dtos;

/// <summary>
/// 商品品牌
/// </summary>
public partial class BrandUpdateCommand : BaseDto<long>, IRequest<BrandDto>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }  = null!;
    
}
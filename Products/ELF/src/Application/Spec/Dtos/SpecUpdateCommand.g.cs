#nullable enable
// 自动生成代码，请不要修改
namespace Dtos;

/// <summary>
/// 商品规格名称
/// </summary>
public partial class SpecUpdateCommand : BaseDto<long>, IRequest<SpecDto>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }  = default!;
    
}
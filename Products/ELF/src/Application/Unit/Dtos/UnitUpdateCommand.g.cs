#nullable enable
// 自动生成代码，请不要修改
namespace Dtos;

/// <summary>
/// 计量单位
/// </summary>
public partial class UnitUpdateCommand : BaseDto<long>, IRequest<UnitDto>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }  = default!;
    
}
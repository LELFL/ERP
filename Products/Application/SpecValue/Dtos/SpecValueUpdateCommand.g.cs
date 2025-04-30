#nullable enable
// 自动生成代码，请不要修改
namespace Dtos;

/// <summary>
/// 规格值
/// </summary>
public partial class SpecValueUpdateCommand : BaseDto<long>, IRequest<SpecValueDto>
{
    /// <summary>
    /// 规格值
    /// </summary>
    public string Value { get; set; }  = default!;
    
    /// <summary>
    /// 规格ID
    /// </summary>
    public long? SpecId { get; set; } 
    
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }  = 0;
    
}
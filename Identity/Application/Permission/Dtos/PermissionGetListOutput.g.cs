#nullable enable
// 自动生成代码，请不要修改
namespace Dtos;

/// <summary>
/// 权限
/// </summary>
public partial class PermissionGetListOutput : BaseDto<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }  = default!;
    
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }  = default!;
    
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; } 
    
    /// <summary>
    /// 父级权限
    /// </summary>
    public long? ParentId { get; set; } 
    
}

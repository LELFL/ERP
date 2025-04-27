#nullable enable
// 自动生成代码，请不要修改
namespace Dtos;

/// <summary>
/// 权限
/// </summary>
public partial class PermissionDto : BaseDto<long>
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
    /// 父级权限ID
    /// </summary>
    public PermissionDto? Parent { get; set; } 
    
    /// <summary>
    /// 父级权限
    /// </summary>
    public long? ParentId { get; set; } 
    
    /// <summary>
    /// 子权限
    /// </summary>
    public List<PermissionDto> Children { get; set; }  = new();
    
}